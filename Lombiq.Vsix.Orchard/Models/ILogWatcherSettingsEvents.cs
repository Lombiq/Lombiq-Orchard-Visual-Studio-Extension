using System;

namespace Lombiq.Vsix.Orchard.Models
{
    /// <summary>
    /// Log watcher settings-related events.
    /// </summary>
    public interface ILogWatcherSettingsEvents
    {
        /// <summary>
        /// Fired after the log watcher settings have been updated.
        /// </summary>
        event EventHandler<LogWatcherSettingsUpdatedEventArgs> SettingsUpdated;
    }
}
