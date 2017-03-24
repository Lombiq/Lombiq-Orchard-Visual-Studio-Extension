using Lombiq.Vsix.Orchard.Models;

namespace Lombiq.Vsix.Orchard.Services
{
    /// <summary>
    /// Service for accessing the log watcher settings.
    /// </summary>
    public interface ILogWatcherSettingsAccessor
    {
        /// <summary>
        /// Returns the log watcher settings necessary for watching the log file.
        /// </summary>
        /// <returns>Log watcher settings.</returns>
        ILogWatcherSettings GetSettings();
    }
}
