using System;
using System.IO;

namespace Lombiq.Vsix.Orchard.Services
{
    public abstract class LogFileWatcherBase : ILogWatcher, IDisposable
    {
        private readonly FileSystemWatcher _fileSystemWatcher;
        private bool _isWatching;


        public event EventHandler LogUpdated;


        public LogFileWatcherBase()
        {
            _fileSystemWatcher = new FileSystemWatcher();
        }

        public virtual void StartWatching()
        {
            if (_isWatching) return;

            var logFileName = GetLogFileName();
            _fileSystemWatcher.Path = Path.GetDirectoryName(logFileName);;
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileSystemWatcher.Filter = Path.GetFileName(logFileName);
            _fileSystemWatcher.EnableRaisingEvents = true;

            _fileSystemWatcher.Changed += FileChanged;

            _isWatching = true;
        }

        public virtual void StopWatching()
        {
            if (!_isWatching) return;

            _fileSystemWatcher.Changed -= FileChanged;

            _isWatching = false;
        }

        public virtual void Dispose()
        {
            StopWatching();

            _fileSystemWatcher.Dispose();
        }


        protected abstract string GetLogFileName();
        
        protected virtual void FileChanged(object sender, FileSystemEventArgs e)
        {
            LogUpdated(this, e);
        }
    }
}
