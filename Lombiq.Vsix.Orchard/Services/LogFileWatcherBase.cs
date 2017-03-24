using Lombiq.Vsix.Orchard.Models;
using System;
using System.IO;

namespace Lombiq.Vsix.Orchard.Services
{
    public abstract class LogFileWatcherBase : ILogFileWatcher
    {
        private readonly FileSystemWatcher _fileSystemWatcher;
        private bool _isWatching;


        public event EventHandler<ILogChangedContext> LogUpdated;


        public LogFileWatcherBase()
        {
            _fileSystemWatcher = new FileSystemWatcher();
        }


        public abstract string GetLogFileName();

        public virtual void StartWatching()
        {
            if (_isWatching) return;

            var logFileName = GetLogFileName();
            _fileSystemWatcher.Path = Path.GetDirectoryName(logFileName);;
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;
            _fileSystemWatcher.Filter = Path.GetFileName(logFileName);
            _fileSystemWatcher.EnableRaisingEvents = true;

            _fileSystemWatcher.Changed += LogFileUpdated;
            _fileSystemWatcher.Created += LogFileUpdated;
            _fileSystemWatcher.Deleted += LogFileUpdated;
            _fileSystemWatcher.Renamed += LogFileUpdated;

            _isWatching = true;
        }

        public virtual void StopWatching()
        {
            if (!_isWatching) return;

            _fileSystemWatcher.Changed -= LogFileUpdated;
            _fileSystemWatcher.Created -= LogFileUpdated;
            _fileSystemWatcher.Deleted -= LogFileUpdated;
            _fileSystemWatcher.Renamed -= LogFileUpdated;

            _isWatching = false;
        }

        public virtual bool HasContent()
        {
            var errorLogFileInfo = new FileInfo(GetLogFileName());

            return errorLogFileInfo.Exists && errorLogFileInfo.Length > 0;
        }

        public virtual void Dispose()
        {
            StopWatching();

            _fileSystemWatcher.Dispose();
        }
        
        
        protected virtual void LogFileUpdated(object sender, FileSystemEventArgs e)
        {
            LogUpdated(
                this, 
                new LogChangedEventArgs
                {
                    HasContent = HasContent(),
                    FileName = GetLogFileName()
                });
        }
    }
}
