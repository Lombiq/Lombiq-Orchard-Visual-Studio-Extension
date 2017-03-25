using EnvDTE;
using Lombiq.Vsix.Orchard.Models;
using System;
using System.IO;
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
        private ILogFileStatus _previousLogFileStatus;


        public event EventHandler<LogChangedEventArgs> LogUpdated;


        public OrchardErrorLogFileWatcher(ILogWatcherSettingsAccessor logWatcherSettingsAccessor, DTE dte)
        {
            _logWatcherSettingsAccessor = logWatcherSettingsAccessor;
            _dte = dte;
            _timer = new Timer();
        }


        public void StartWatching()
        {
            if (_isWatching) return;

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
        }

        public ILogFileStatus GetLogFileStatus()
        {
            var fileInfo = new FileInfo(GetLogFileName());

            return new LogFileStatus
            {
                Exists = fileInfo.Exists,
                HasContent = fileInfo.Exists && fileInfo.Length > 0,
                FileName = fileInfo.FullName
            };
        }

        public void Dispose()
        {
            StopWatching();

            _timer.Dispose();
        }


        private string GetLogFileName()
        {
            var logFilePath = _logWatcherSettingsAccessor.GetSettings().LogFileFolderPath;
            var solutionPath = _dte.Solution != null && _dte.Solution.IsOpen ? Path.GetDirectoryName(_dte.Solution.FileName) : "";
            var errorLogFileName = "orchard-error-" + DateTime.Today.ToString("yyyy.MM.dd") + ".log";

            return Path.Combine(solutionPath, logFilePath, errorLogFileName);
        }
        
        private void LogWatcherTimerElapsedCallback(object sender, ElapsedEventArgs e)
        {
            var logFileStatus = GetLogFileStatus();

            if (logFileStatus != _previousLogFileStatus)
            {
                LogUpdated?.Invoke(
                    this,
                    new LogChangedEventArgs
                    {
                        LogFileStatus = logFileStatus
                    });

                _previousLogFileStatus = logFileStatus;
            }
        }
    }
}
