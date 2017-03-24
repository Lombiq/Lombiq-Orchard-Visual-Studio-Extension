namespace Lombiq.Vsix.Orchard.Models
{
    /// <summary>
    /// Contains settings for the log watcher feature like log file path.
    /// </summary>
    public interface ILogWatcherSettings
    {
        /// <summary>
        /// Relative path of the folder where the log file is located.
        /// </summary>
        string LogFileFolderPath { get; set; }
    }
}
