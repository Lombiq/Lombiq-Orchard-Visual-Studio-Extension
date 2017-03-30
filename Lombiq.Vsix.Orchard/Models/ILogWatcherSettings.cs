using System;

namespace Lombiq.Vsix.Orchard.Models
{
    /// <summary>
    /// Contains settings for the log watcher feature like log file path.
    /// </summary>
    public interface ILogWatcherSettings
    {
        /// <summary>
        /// Fired after the log watcher settings have been updated.
        /// </summary>
        event EventHandler<LogWatcherSettingsUpdatedEventArgs> SettingsUpdated;

        /// <summary>
        /// Indicated whether the log watcher is enabled and need to check if the log file has new entries. 
        /// </summary>
        bool LogWatcherEnabled { get; }

        /// <summary>
        /// Relative path of the folder where the log file is located.
        /// </summary>
        string LogFileFolderPath { get; }
    }
}
