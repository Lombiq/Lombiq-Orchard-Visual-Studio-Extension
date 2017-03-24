using EnvDTE;
using Lombiq.Vsix.Orchard.Models;
using System;
using System.IO;

namespace Lombiq.Vsix.Orchard.Services
{
    public class OrchardErrorLogFileWatcher : ILogFileWatcher
    {
        private readonly FileSystemWatcher _fileSystemWatcher;
        private readonly ILogWatcherSettingsAccessor _logWatcherSettingsAccessor;
        private readonly DTE _dte;
        private bool _isWatching;


        public event EventHandler<ILogChangedContext> LogUpdated;


        public OrchardErrorLogFileWatcher(ILogWatcherSettingsAccessor logWatcherSettingsAccessor, DTE dte)
        {
            _logWatcherSettingsAccessor = logWatcherSettingsAccessor;
            _dte = dte;

            _fileSystemWatcher = new FileSystemWatcher();
        }


        public void StartWatching()
        {
            if (_isWatching) return;

            var logFileName = GetLogFileName();
            UpdateFileNameInWatcher(logFileName);
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;
            _fileSystemWatcher.EnableRaisingEvents = true;

            _fileSystemWatcher.Changed += LogFileUpdated;
            _fileSystemWatcher.Created += LogFileUpdated;
            _fileSystemWatcher.Deleted += LogFileUpdated;
            _fileSystemWatcher.Renamed += LogFileUpdated;

            _isWatching = true;

            GetSettings().SettingsUpdated += SettingsUpdated;
        }

        public void StopWatching()
        {
            if (!_isWatching) return;

            _fileSystemWatcher.Changed -= LogFileUpdated;
            _fileSystemWatcher.Created -= LogFileUpdated;
            _fileSystemWatcher.Deleted -= LogFileUpdated;
            _fileSystemWatcher.Renamed -= LogFileUpdated;

            _isWatching = false;

            GetSettings().SettingsUpdated -= SettingsUpdated;
        }

        public string GetLogFileName()
        {
            var logFilePath = _logWatcherSettingsAccessor.GetSettings().LogFileFolderPath;
            var solutionPath = _dte.Solution == null ? "" : Path.GetDirectoryName(_dte.Solution.FileName);
            var errorLogFileName = "orchard-error-" + DateTime.Today.ToString("yyyy.MM.dd") + ".log";

            return Path.Combine(solutionPath, logFilePath, errorLogFileName);
        }

        public bool HasContent()
        {
            var errorLogFileInfo = new FileInfo(GetLogFileName());

            return errorLogFileInfo.Exists && errorLogFileInfo.Length > 0;
        }

        public void Dispose()
        {
            StopWatching();

            _fileSystemWatcher.Dispose();
        }


        private ILogWatcherSettings GetSettings()
        {
            return _logWatcherSettingsAccessor.GetSettings();
        }

        private void UpdateFileNameInWatcher(string fileName)
        {
            _fileSystemWatcher.Path = Path.GetDirectoryName(fileName);
            _fileSystemWatcher.Filter = Path.GetFileName(fileName);
        }

        private void LogFileUpdated(object sender, FileSystemEventArgs e)
        {
            LogUpdated(
                this,
                new LogChangedEventArgs
                {
                    HasContent = HasContent(),
                    FileName = GetLogFileName()
                });
        }

        private void SettingsUpdated(object sender, EventArgs e)
        {
            UpdateFileNameInWatcher(GetLogFileName());
        }
    }
}
