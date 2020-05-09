﻿using Lombiq.Vsix.Orchard.Models;
using System;
using System.Threading.Tasks;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    /// <summary>
    /// Watches a specific log file and fires events if the log file has changed or removed.
    /// </summary>
    public interface ILogFileWatcher : IDisposable
    {
        /// <summary>
        /// Event that is fired if the log file has changed or removed. It's only fired if the watching is active.
        /// </summary>
        event EventHandler<LogChangedEventArgs> LogUpdated;

        /// <summary>
        /// Starts watching the log file.
        /// </summary>
        Task StartWatching();

        /// <summary>
        /// Stops watching the log file.
        /// </summary>
        void StopWatching();

        /// <summary>
        /// Checks if the log file exists in any possible log paths and has content.
        /// </summary>
        /// <returns>Log file status and details.</returns>
        Task<ILogFileStatus> GetLogFileStatus();
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
