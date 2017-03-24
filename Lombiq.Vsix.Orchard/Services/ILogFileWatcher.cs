using Lombiq.Vsix.Orchard.Models;
using System;

namespace Lombiq.Vsix.Orchard.Services
{
    /// <summary>
    /// Watches a specific log file and fires events if the log file has changed or removed.
    /// </summary>
    public interface ILogFileWatcher : IDisposable
    {
        /// <summary>
        /// Event that is fired if the log file has changed or removed. It's only fired if the watching is active.
        /// </summary>
        event EventHandler<ILogChangedContext> LogUpdated;

        /// <summary>
        /// Starts watching the log file.
        /// </summary>
        void StartWatching();

        /// <summary>
        /// Stops watching the log file.
        /// </summary>
        void StopWatching();

        /// <summary>
        /// Returns true if the log file exists and is not empty.
        /// </summary>
        /// <returns>Returns true if the log file exists and is not empty.</returns>
        bool HasContent();

        /// <summary>
        /// Returns the file name of the log file being watched by this log watcher instance.
        /// </summary>
        /// <returns>Full file name of the log file.</returns>
        string GetLogFileName();
    }


    public static class LogFileWatcherExtensions
    {
        /// <summary>
        /// Stops and starts watching of the log file.
        /// </summary>
        public static void RestartWatching(this ILogFileWatcher logFileWatcher)
        {
            logFileWatcher.StopWatching();
            logFileWatcher.StartWatching();
        }
    }
}
