using EnvDTE;
using Lombiq.Vsix.Orchard.Models;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
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


        public OrchardErrorLogFileWatcher(IServiceProvider serviceProvider)
        {
            _logWatcherSettingsAccessor = serviceProvider.GetService<ILogWatcherSettingsAccessor>();
            _dte = Package.GetGlobalService(typeof(SDTE)) as DTE;

            _timer = new Timer();
        }


        public void StartWatching()
        {
            if (_isWatching) return;

            _previousLogFileStatus = GetLogFileStatus();

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

            _previousLogFileStatus = null;
        }

        public ILogFileStatus GetLogFileStatus()
        {
            var fileInfo = new FileInfo(GetLogFileName());

            return new LogFileStatus
            {
                Exists = fileInfo.Exists,
                HasContent = fileInfo.Exists && fileInfo.Length > 0,
                Path = fileInfo.FullName,
                LastUpdatedUtc = fileInfo.Exists ? (DateTime?)fileInfo.LastWriteTimeUtc : null
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
            var solutionPath = IsSolutionOpen() ? Path.GetDirectoryName(_dte.Solution.FileName) : "";
            var errorLogFileName = "orchard-error-" + DateTime.Today.ToString("yyyy.MM.dd") + ".log";

            return Path.Combine(solutionPath, logFilePath, errorLogFileName);
        }
        
        private void LogWatcherTimerElapsedCallback(object sender, ElapsedEventArgs e)
        {
            if (!IsSolutionOpen()) return;

            var logFileStatus = GetLogFileStatus();

            if (!logFileStatus.Equals(_previousLogFileStatus))
            {
                LogUpdated?.Invoke(this, new LogChangedEventArgs { LogFileStatus = logFileStatus });

                _previousLogFileStatus = logFileStatus;
            }
        }

        private bool IsSolutionOpen() => _dte.Solution.IsOpen;
    }
}
