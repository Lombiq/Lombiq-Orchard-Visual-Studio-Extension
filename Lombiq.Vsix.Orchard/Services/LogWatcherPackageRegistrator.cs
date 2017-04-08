using EnvDTE;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Helpers;
using Lombiq.Vsix.Orchard.Models;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;

namespace Lombiq.Vsix.Orchard.Services
{
    public class LogWatcherPackageRegistrator : IPackageRegistrator
    {
        private readonly Lazy<ILogWatcherSettings> _lazyLogWatcherSettings;
        private readonly ILogFileWatcher _logWatcher;
        private DTE _dte;
        private IMenuCommandService _menuCommandService;
        private OleMenuCommand _openErrorLogCommand;
        private CommandBar _orchardLogWatcherToolbar;
        private bool _hasSeenErrorLogUpdate;


        public LogWatcherPackageRegistrator(
            ILogWatcherSettingsAccessor logWatcherSettingsAccessor,
            ILogFileWatcher logFileWatcher)
        {
            _lazyLogWatcherSettings = new Lazy<ILogWatcherSettings>(logWatcherSettingsAccessor.GetSettings);
            _logWatcher = logFileWatcher;
        }


        public void RegisterCommands(DTE dte, IMenuCommandService menuCommandService)
        {
            _dte = dte;
            _menuCommandService = menuCommandService;
            
            _hasSeenErrorLogUpdate = true;

            _logWatcher.LogUpdated += LogFileUpdatedCallback;
            _lazyLogWatcherSettings.Value.SettingsUpdated += LogWatcherSettingsUpdatedCallback;

            // Initialize "Open Error Log" toolbar button.
            _openErrorLogCommand = new OleMenuCommand(
                OpenErrorLogCallback,
                new CommandID(
                    PackageGuids.LombiqOrchardVisualStudioExtensionCommandSetGuid,
                    (int)CommandIds.OpenErrorLogCommandId));
            _openErrorLogCommand.BeforeQueryStatus += OpenErrorLogCommandBeforeQueryStatusCallback;

            _menuCommandService.AddCommand(_openErrorLogCommand);

            // Store Log Watcher toolbar in a variable to be able to show or hide depending on the Log Watcher settings.
            _orchardLogWatcherToolbar = ((CommandBars)_dte.CommandBars)[CommandBarNames.OrchardLogWatcherToolbarName];

            if (_lazyLogWatcherSettings.Value.LogWatcherEnabled)
            {
                _openErrorLogCommand.Visible = true;

                StartLogFileWatching();
            }
        }

        public void Dispose()
        {
            _logWatcher.LogUpdated -= LogFileUpdatedCallback;
            _lazyLogWatcherSettings.Value.SettingsUpdated -= LogWatcherSettingsUpdatedCallback;

            _logWatcher.Dispose();
        }


        private void OpenErrorLogCommandBeforeQueryStatusCallback(object sender, EventArgs e) =>
            UpdateOpenErrorLogCommandAccessibilityAndText();

        private void LogFileUpdatedCallback(object sender, LogChangedEventArgs context)
        {
            _hasSeenErrorLogUpdate = !context.LogFileStatus.HasContent;

            UpdateOpenErrorLogCommandAccessibilityAndText(context.LogFileStatus);
        }

        private void OpenErrorLogCallback(object sender, EventArgs e)
        {
            _hasSeenErrorLogUpdate = true;

            var status = GetLogFileStatus();

            if (status.Exists)
            {
                System.Diagnostics.Process.Start(status.Path);
            }
            else
            {
                DialogHelpers.Error("The log file doesn't exists.", "Open Orchard Error Log");
            }

            UpdateOpenErrorLogCommandAccessibilityAndText();
        }

        private void LogWatcherSettingsUpdatedCallback(object sender, LogWatcherSettingsUpdatedEventArgs e)
        {
            _orchardLogWatcherToolbar.Visible = e.Settings.LogWatcherEnabled;

            if (!e.Settings.LogWatcherEnabled)
            {
                StopLogFileWatching();
            }
            else
            {
                StartLogFileWatching();
            }

            UpdateOpenErrorLogCommandAccessibilityAndText();
        }

        private void UpdateOpenErrorLogCommandAccessibilityAndText(ILogFileStatus logFileStatus = null)
        {
            if (!_dte.Solution.IsOpen)
            {
                _openErrorLogCommand.Enabled = false;
                _openErrorLogCommand.Text = "Solution is not open";
            }
            else if (_lazyLogWatcherSettings.Value.LogWatcherEnabled && 
                ((logFileStatus?.HasContent ?? false) || 
                !_hasSeenErrorLogUpdate))
            {
                _openErrorLogCommand.Enabled = true;
                _openErrorLogCommand.Text = "Open Orchard error log";
            }
            else
            {
                _openErrorLogCommand.Enabled = false;
                _openErrorLogCommand.Text = "Orchard error log doesn't exist or hasn't been updated";
            }
        }

        private ILogFileStatus GetLogFileStatus() =>
            _logWatcher.GetLogFileStatus();

        private void StartLogFileWatching() =>
            _logWatcher.StartWatching();

        private void StopLogFileWatching() =>
            _logWatcher.StopWatching();
    }
}
