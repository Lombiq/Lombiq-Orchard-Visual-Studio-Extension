using Lombiq.Vsix.Orchard.Constants;
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
        private const string DefaultLogFileFolderPath = @"src\Orchard.Web\App_Data\Logs";


        public event EventHandler SettingsUpdated;


        public LogWatcherOptionsPage()
        {
            LogFileFolderPath = DefaultLogFileFolderPath;
        }

        [DisplayName("Log file folder path")]
        [Category("Log Watcher Options")]
        [Description("Relative path where the log files are located. It must be relative to the solution file that is currently opened.")]
        public string LogFileFolderPath { get; set; }

        
        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);

            SettingsUpdated?.Invoke(this, e);
        }
    }
}
