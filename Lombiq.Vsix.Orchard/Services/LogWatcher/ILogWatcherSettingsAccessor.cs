using Lombiq.Vsix.Orchard.Models;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    /// <summary>
    /// Service for accessing the Log Watcher settings.
    /// </summary>
    public interface ILogWatcherSettingsAccessor
    {
        /// <summary>
        /// Returns the Log Watcher settings necessary for watching the log file.
        /// </summary>
        /// <returns>Log Watcher settings.</returns>
        ILogWatcherSettings GetSettings();
    }
}
