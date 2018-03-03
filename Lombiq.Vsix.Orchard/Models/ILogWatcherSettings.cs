using System;
using System.Collections.Generic;

namespace Lombiq.Vsix.Orchard.Models
{
    /// <summary>
    /// Contains settings for the Log Watcher feature like log file path.
    /// </summary>
    public interface ILogWatcherSettings
    {
        /// <summary>
        /// Fired after the Log Watcher settings have been updated.
        /// </summary>
        event EventHandler<LogWatcherSettingsUpdatedEventArgs> SettingsUpdated;

        /// <summary>
        /// Indicates whether the Log Watcher is enabled and need to check if the log file has new entries. 
        /// </summary>
        bool LogWatcherEnabled { get; }

        /// <summary>
        /// Returns the relative paths of the folders where the log files can be located.
        /// </summary>
        IEnumerable<string> GetLogFileFolderPaths();
    }
}
