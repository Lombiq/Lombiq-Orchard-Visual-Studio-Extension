using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Helpers;
using Lombiq.Vsix.Orchard.Models;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Lombiq.Vsix.Orchard.Options
{
    [Guid(PackageGuids.LogWatcherOptionsPageGuidString)]
    public class LogWatcherOptionsPage : DialogPage, ILogWatcherSettings
    {
        private const bool DefaultLogWatcherEnabled = true;
        private const string DefaultLogFileFolderPath = @"Orchard.Web\App_Data\Logs";


        public event EventHandler<LogWatcherSettingsUpdatedEventArgs> SettingsUpdated;


        public LogWatcherOptionsPage()
        {
            LogWatcherEnabled = DefaultLogWatcherEnabled;
            LogFileFolderPath = DefaultLogFileFolderPath;
        }


        [DisplayName("Enabled")]
        [Category("General Log Watcher Options")]
        [Description("Enable/disable log watcher feature. With this option turned off the log watcher won't check if the log file has any new entry.")]
        public bool LogWatcherEnabled { get; set; }

        [DisplayName("Log file folder path")]
        [Category("General Log Watcher Options")]
        [Description("Relative path where the log files are located. It must be relative to the solution file that is currently opened.")]
        public string LogFileFolderPath { get; set; }


        protected override void OnDeactivate(CancelEventArgs e)
        {
            base.OnDeactivate(e);

            // Check if the log file folder path is empty only if the feature is enabled. Don't allow invalid paths at all.
            if (LogWatcherEnabled && string.IsNullOrEmpty(LogFileFolderPath))
            {
                DialogHelpers.Warning("Log file folder path is required if the feature is enabled.", "Log watcher settings");

                e.Cancel = true;
            }

            if (!Uri.IsWellFormedUriString(LogFileFolderPath, UriKind.Relative))
            {
                DialogHelpers.Warning("The given log file folder path is invalid.", "Log watcher settings");

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
