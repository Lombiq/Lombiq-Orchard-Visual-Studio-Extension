using Lombiq.Vsix.Orchard.Models;
using System;

namespace Lombiq.Vsix.Orchard.Services
{
    public interface ILogFileWatcher : IDisposable
    {
        event EventHandler<ILogChangedContext> LogUpdated;

        void StartWatching();
        void StopWatching();
        bool HasContent();
        string GetLogFileName();
    }


    public static class LogFileWatcherExtensions
    {
        public static void RestartWatching(this ILogFileWatcher logFileWatcher)
        {
            logFileWatcher.StopWatching();
            logFileWatcher.StartWatching();
        }
    }
}
