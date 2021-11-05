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
        /// Log file name search pattern (as offered by Directory.EnumerateFiles()) to look for files in the
        /// directories provided by <see cref="GetLogFileFolderPaths"/>.
        /// </summary>
        string LogFileNameSearchPattern { get; }

        /// <summary>
        /// The color, as a hex value or noun, to use with an attached BlinkStick USB LED stick if present.
        /// </summary>
        string BlinkStickColor { get; }

        /// <summary>
        /// Indicates whether an attached BlinkStick USB LED stick, if present, will blink (<c>true</c>) or light up
        /// continuously (<c>false</c>) when a new error log entry is detected.
        /// </summary>
        bool BlinkBlinkStick { get; }

        /// <summary>
        /// Returns the relative paths of the folders where the log files can be located.
        /// </summary>
        IEnumerable<string> GetLogFileFolderPaths();
    }
}
