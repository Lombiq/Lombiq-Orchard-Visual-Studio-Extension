using EnvDTE;
using Lombiq.Vsix.Orchard.Models;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    public abstract class LogFileWatcherBase
    {
        private const int DefaultLogWatcherTimerIntervalInMilliseconds = 1000;


        private readonly ILogWatcherSettingsAccessor _logWatcherSettingsAccessor;
        private readonly DTE _dte;
        private readonly Timer _timer;
        private bool _isWatching;
        private ILogFileStatus _previousLogFileStatus;


        public event EventHandler<LogChangedEventArgs> LogUpdated;


        public LogFileWatcherBase(IServiceProvider serviceProvider)
        {
            _logWatcherSettingsAccessor = serviceProvider.GetService<ILogWatcherSettingsAccessor>();
            _dte = Package.GetGlobalService(typeof(SDTE)) as DTE;

            _timer = new Timer();
        }


        protected abstract string GetLogFileName();

        public virtual void StartWatching()
        {
            if (_isWatching) return;

            _previousLogFileStatus = GetLogFileStatus();

            _timer.Interval = DefaultLogWatcherTimerIntervalInMilliseconds;
            _timer.AutoReset = true;
            _timer.Elapsed += LogWatcherTimerElapsedCallback;
            _timer.Start();

            _isWatching = true;
        }

        public virtual void StopWatching()
        {
            if (!_isWatching) return;

            _timer.Stop();
            _timer.Elapsed -= LogWatcherTimerElapsedCallback;

            _isWatching = false;

            _previousLogFileStatus = null;
        }

        public ILogFileStatus GetLogFileStatus()
        {
            var logFilePath = GetExistingLogFilePath();

            if (string.IsNullOrEmpty(logFilePath)) return null;

            var fileInfo = new FileInfo(logFilePath);
            
            return new LogFileStatus
            {
                Exists = fileInfo.Exists,
                HasContent = fileInfo.Exists && fileInfo.Length > 0,
                Path = fileInfo.FullName,
                LastUpdatedUtc = fileInfo.Exists ? (DateTime?)fileInfo.LastWriteTimeUtc : null
            };
        }

        public virtual void Dispose()
        {
            StopWatching();

            _timer.Dispose();
        }


        protected virtual string GetExistingLogFilePath()
        {
            var logFilePaths = _logWatcherSettingsAccessor.GetSettings().GetLogFileFolderPaths();
            var solutionPath = IsSolutionOpen() && !string.IsNullOrEmpty(_dte.Solution.FileName) ? 
                Path.GetDirectoryName(_dte.Solution.FileName) : "";

            return GetAllMatchingPaths(solutionPath, logFilePaths, GetLogFileName()).FirstOrDefault();
        }

        protected virtual void LogWatcherTimerElapsedCallback(object sender, ElapsedEventArgs e)
        {
            if (!IsSolutionOpen()) return;

            var logFileStatus = GetLogFileStatus();

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
                        Path = _previousLogFileStatus.Path
                    }
                });
            }
            // Log file has been added or changed.
            else if (_previousLogFileStatus == null && logFileStatus != null ||
                logFileStatus != null && !logFileStatus.Equals(_previousLogFileStatus))
            {
                LogUpdated?.Invoke(this, new LogChangedEventArgs { LogFileStatus = logFileStatus });
            }

            _previousLogFileStatus = logFileStatus;
        }

        protected bool IsSolutionOpen() => _dte.Solution.IsOpen;
        
        protected static IEnumerable<string> GetAllMatchingPaths(
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
