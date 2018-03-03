using EnvDTE;
using Lombiq.Vsix.Orchard.Models;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace Lombiq.Vsix.Orchard.Services
{
    public class OrchardErrorLogFileWatcher : ILogFileWatcher
    {
        private const int DefaultLogWatcherTimerIntervalInMilliseconds = 1000;


        private readonly ILogWatcherSettingsAccessor _logWatcherSettingsAccessor;
        private readonly DTE _dte;
        private readonly Timer _timer;
        private bool _isWatching;
        private IList<ILogFileStatus> _previousLogFileStatuses;


        public event EventHandler<LogChangedEventArgs> LogUpdated;


        public OrchardErrorLogFileWatcher(IServiceProvider serviceProvider)
        {
            _logWatcherSettingsAccessor = serviceProvider.GetService<ILogWatcherSettingsAccessor>();
            _dte = Package.GetGlobalService(typeof(SDTE)) as DTE;

            _timer = new Timer();
        }


        public void StartWatching()
        {
            if (_isWatching) return;

            _previousLogFileStatuses = GetLogFileStatuses().ToList();

            _timer.Interval = DefaultLogWatcherTimerIntervalInMilliseconds;
            _timer.AutoReset = true;
            _timer.Elapsed += LogWatcherTimerElapsedCallback;
            _timer.Start();

            _isWatching = true;
        }

        public void StopWatching()
        {
            if (!_isWatching) return;

            _timer.Stop();
            _timer.Elapsed -= LogWatcherTimerElapsedCallback;

            _isWatching = false;

            _previousLogFileStatuses = Enumerable.Empty<ILogFileStatus>().ToList();
        }

        public IEnumerable<ILogFileStatus> GetLogFileStatuses()
        {
            var fileInfos = GetLogFileNames().Select(fileName => new FileInfo(fileName));
            
            return fileInfos.Select(fileInfo =>
                new LogFileStatus
                {
                    Exists = fileInfo.Exists,
                    HasContent = fileInfo.Exists && fileInfo.Length > 0,
                    Path = fileInfo.FullName,
                    LastUpdatedUtc = fileInfo.Exists ? (DateTime?)fileInfo.LastWriteTimeUtc : null
                });
        }

        public void Dispose()
        {
            StopWatching();

            _timer.Dispose();
        }


        private IEnumerable<string> GetLogFileNames()
        {
            var logFilePaths = _logWatcherSettingsAccessor.GetSettings().GetLogFileFolderPaths();
            var solutionPath = IsSolutionOpen() ? Path.GetDirectoryName(_dte.Solution.FileName) : "";
            var errorLogFileName = "orchard-error-" + DateTime.Today.ToString("yyyy.MM.dd") + ".log";

            return logFilePaths.Select(path => Path.Combine(solutionPath, path, errorLogFileName));
        }
        
        private void LogWatcherTimerElapsedCallback(object sender, ElapsedEventArgs e)
        {
            if (!IsSolutionOpen()) return;

            var logFileStatuses = GetLogFileStatuses().ToList();
            var previous = _previousLogFileStatuses;

            for (var i = 0; i < logFileStatuses.ToList().Count; i++)
            {
                var currentStatus = logFileStatuses[i];
                
                if (!currentStatus.Equals(previous[i]))
                {
                    LogUpdated?.Invoke(this, new LogChangedEventArgs { LogFileStatus = currentStatus });

                    _previousLogFileStatuses = logFileStatuses;
                }
            }
        }

        private bool IsSolutionOpen() => _dte.Solution.IsOpen;
    }
}
