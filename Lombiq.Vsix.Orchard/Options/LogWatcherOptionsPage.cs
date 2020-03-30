using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Helpers;
using Lombiq.Vsix.Orchard.Models;
using Lombiq.Vsix.Orchard.Services.LogWatcher;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Lombiq.Vsix.Orchard.Options
{
    [Guid(PackageGuids.LogWatcherOptionsPageGuidString)]
    public class LogWatcherOptionsPage : DialogPage, ILogWatcherSettings
    {
        public event EventHandler<LogWatcherSettingsUpdatedEventArgs> SettingsUpdated;

        [DisplayName("Enabled")]
        [Category("General Log Watcher Options")]
        [Description("Enable/disable Log Watcher feature. With this option turned off the Log Watcher won't check if the log file has any new entry.")]
        public bool LogWatcherEnabled { get; set; } = true;

        [DisplayName("Log file folder path")]
        [Category("General Log Watcher Options")]
        [Description("List of relative paths where the log files are located. Values must be relative to the solution file that is currently opened. Wildcards can be used. Use the '|' character to separate values. Example: App_Data/logs|*.Web/App_Data/logs.")]
        public string LogFileFolderPathsSerialized { get; set; } =
            @"Orchard.Web/App_Data/Logs|src/OrchardCore.Cms.Web/App_Data/logs|*.Web/App_Data/logs|src/*.Web/App_Data/logs";

        [DisplayName("Color for the BlinkStick LED stick")]
        [Category("Log Watcher BlinkStick Options")]
        [Description("Set the color for the BlinkStick USB LED stick here. Use either a hex value or one of the names listed here: https://github.com/arvydas/BlinkStickDotNet/blob/master/BlinkStickDotNet/RgbColor.cs#L30.")]
        public string BlinkStickColor { get; set; } = "red";

        [DisplayName("Blink BlinkStick LED stick")]
        [Category("Log Watcher BlinkStick Options")]
        [Description("You can use your BlinkStick USB LED stick (see the Readme) so it lights up when a new log entry is detected. Enable/disable whether to make it blink when a new long entry is detected. When disabled the BlinkStick will light up continuously.")]
        public bool BlinkBlinkStick { get; set; } = false;


        public IEnumerable<string> GetLogFileFolderPaths() =>
            LogFileFolderPathsSerialized?
                .Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(value => value.Trim()) ?? Enumerable.Empty<string>();


        protected override void OnDeactivate(CancelEventArgs e)
        {
            base.OnDeactivate(e);

            // Check if the log file folder path is empty only if the feature is enabled. Don't allow invalid paths at all.

            if (LogWatcherEnabled)
            {
                if (string.IsNullOrEmpty(LogFileFolderPathsSerialized))
                {
                    DialogHelpers.Warning("Log file folder path is required if the feature is enabled.", "Log Watcher settings");
                    e.Cancel = true;
                }

                if (!BlinkStickManager.IsValidColor(BlinkStickColor))
                {
                    DialogHelpers.Warning(
                        "The given BlinkStick color is invalid. Please use a valid hex value or one of the names listed here: https://github.com/arvydas/BlinkStickDotNet/blob/master/BlinkStickDotNet/RgbColor.cs#L30.",
                        "Log Watcher settings");
                    e.Cancel = true;
                }
            }

            if (GetLogFileFolderPaths().Any(path => !Uri.IsWellFormedUriString(path, UriKind.Relative)))
            {
                DialogHelpers.Warning("The given log file folder path is invalid.", "Log Watcher settings");
                e.Cancel = true;
            }
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);

            SettingsUpdated?.Invoke(this, new LogWatcherSettingsUpdatedEventArgs { Settings = this });
        }
    }
}
