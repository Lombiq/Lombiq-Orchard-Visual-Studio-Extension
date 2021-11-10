using EnvDTE;
using Lombiq.Vsix.Orchard.Models;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    public abstract class LogFileWatcherBase : ILogFileWatcher
    {
        private const int DefaultLogWatcherTimerIntervalInMilliseconds = 1000;

        private readonly AsyncPackage _package;
        protected readonly ILogWatcherSettingsAccessor _logWatcherSettingsAccessor;
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Usage",
            "CA2213:Disposable fields should be disposed",
            Justification = "The timer is Disposed from the StopWatching method when the LogFileWatcherBase class is Disposed")]
        private Timer _timer;
        private bool _isWatching;
        private ILogFileStatus _previousLogFileStatus;

        public event EventHandler<LogChangedEventArgs> LogUpdated;

        protected LogFileWatcherBase(AsyncPackage package, ILogWatcherSettingsAccessor logWatcherSettingsAccessor)
        {
            _package = package;
            _logWatcherSettingsAccessor = logWatcherSettingsAccessor;
        }

        protected abstract Task<string> GetLogFileNameAsync();

        public virtual async System.Threading.Tasks.Task StartWatchingAsync()
        {
            if (_isWatching) return;

            _previousLogFileStatus = await GetLogFileStatusAsync();

            // Using this pattern: https://stackoverflow.com/a/684208/220230 to prevent overlapping timer calls.
            // Since Timer callbacks are executed in a ThreadPool thread
            // (https://docs.microsoft.com/en-us/dotnet/standard/threading/timers) they won't block the UI thread.
            _timer = new Timer(TimerCallback, null, 0, Timeout.Infinite);

            _isWatching = true;
        }

        private void TimerCallback(object state) => _ = TimerCallbackAsync();

        private async System.Threading.Tasks.Task TimerCallbackAsync()
        {
            try
            {
                if (!(await _package.GetDteAsync()).SolutionIsOpen()) return;

                var logFileStatus = await GetLogFileStatusAsync();

                // Log file has been deleted.
                if (logFileStatus == null && _previousLogFileStatus != null)
                {
                    LogUpdated?.Invoke(this, new LogChangedEventArgs
                    {
                        LogFileStatus = new LogFileStatus
                        {
                            Exists = false,
                            LastUpdatedUtc = DateTime.UtcNow,
                            HasContent = false,
                            Path = _previousLogFileStatus.Path,
                        },
                    });
                }

                // Log file has been added or changed.
                else if ((_previousLogFileStatus == null && logFileStatus != null) ||
                    (logFileStatus != null && !logFileStatus.Equals(_previousLogFileStatus)))
                {
                    LogUpdated?.Invoke(this, new LogChangedEventArgs { LogFileStatus = logFileStatus });
                }

                _previousLogFileStatus = logFileStatus;
            }
            finally
            {
                try
                {
                    _timer.Change(DefaultLogWatcherTimerIntervalInMilliseconds, Timeout.Infinite);
                }
                catch (ObjectDisposedException)
                {
                    // This can happen when the Log Watcher is disabled. Just swallowing it not to cause any issues.
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Critical Bug",
            "S2952:Classes should \"Dispose\" of members from the classes' own \"Dispose\" methods",
            Justification = "The timer has to be Disposed when the watcher is stopped.")]
        public virtual void StopWatching()
        {
            if (!_isWatching) return;

            _timer.Dispose();

            _isWatching = false;

            _previousLogFileStatus = null;
        }

        public async Task<ILogFileStatus> GetLogFileStatusAsync()
        {
            var logFilePath = await GetExistingLogFilePathAsync();

            if (string.IsNullOrEmpty(logFilePath)) return null;

            var fileInfo = new FileInfo(logFilePath);

            return new LogFileStatus
            {
                Exists = fileInfo.Exists,
                HasContent = fileInfo.Exists && fileInfo.Length > 0,
                Path = fileInfo.FullName,
                LastUpdatedUtc = fileInfo.Exists ? (DateTime?)fileInfo.LastWriteTimeUtc : null,
            };
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopWatching();
            }
        }

        protected virtual async Task<string> GetExistingLogFilePathAsync()
        {
            var logFilePaths = (await _logWatcherSettingsAccessor.GetSettingsAsync()).GetLogFileFolderPaths();
            var dte = await _package.GetDteAsync();
            var solutionPath = dte.SolutionIsOpen() && !string.IsNullOrEmpty(dte.Solution.FileName) ?
                Path.GetDirectoryName(dte.Solution.FileName) : string.Empty;

            var logFileName = await GetLogFileNameAsync();

            if (string.IsNullOrEmpty(logFileName)) return null;

            return GetAllMatchingPaths(solutionPath, logFilePaths, logFileName).FirstOrDefault();
        }

        protected virtual IEnumerable<string> GetAllMatchingPaths(
            string root,
            IEnumerable<string> patterns,
            string logFileName)
        {
            var fullPaths = patterns.Select(pattern => Path.Combine(root, pattern, logFileName).Replace("/", "\\"));

            return fullPaths.SelectMany(fullPath =>
            {
                var parts = fullPath.Split(Path.DirectorySeparatorChar);

                return GetAllMatchingPathsInternal(
                    string.Join(Path.DirectorySeparatorChar.ToString(), parts.Skip(1)),
                    parts[0]);
            });
        }

        private static IEnumerable<string> GetAllMatchingPathsInternal(string pattern, string root)
        {
            var parts = pattern.Split(Path.DirectorySeparatorChar);

            for (var i = 0; i < parts.Length; i++)
            {
                // If this part of the path is a wildcard that needs expanding.
                if (parts[i].Contains('*') || parts[i].Contains('?'))
                {
                    var combined = root +
                        Path.DirectorySeparatorChar +
                        string.Join(Path.DirectorySeparatorChar.ToString(), parts.Take(i));

                    // Create an absolute path up to the current wildcard and check if it exists.
                    if (!Directory.Exists(combined)) return Enumerable.Empty<string>();

                    // If this is the end of the path (a file name).
                    if (i == parts.Length - 1)
                    {
                        return Directory.EnumerateFiles(combined, parts[i], SearchOption.TopDirectoryOnly);
                    }

                    // If this is in the middle of the path (a directory name).
                    else
                    {
                        var directories = Directory.EnumerateDirectories(
                            combined,
                            parts[i],
                            SearchOption.TopDirectoryOnly);
                        var paths = directories.SelectMany(directory =>
                            GetAllMatchingPathsInternal(
                                string.Join(Path.DirectorySeparatorChar.ToString(), parts.Skip(i + 1)),
                                directory));

                        return paths;
                    }
                }
            }

            // If pattern ends in an absolute path with no wildcards in the filename.
            var absolute = root + Path.DirectorySeparatorChar + string.Join(Path.DirectorySeparatorChar.ToString(), parts);
            if (File.Exists(absolute)) return new[] { absolute };

            return Enumerable.Empty<string>();
        }
    }
}
