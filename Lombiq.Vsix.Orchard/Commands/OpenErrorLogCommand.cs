using EnvDTE;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Helpers;
using Lombiq.Vsix.Orchard.Models;
using Lombiq.Vsix.Orchard.Services.LogWatcher;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;

namespace Lombiq.Vsix.Orchard.Commands
{
    internal sealed class OpenErrorLogCommand : IDisposable
    {
        public const int CommandId = CommandIds.OpenErrorLogCommandId;
        public static readonly Guid CommandSet = PackageGuids.LombiqOrchardVisualStudioExtensionCommandSetGuid;


        private readonly IServiceProvider _serviceProvider;
        private readonly IMenuCommandService _menuCommandService;
        private readonly DTE _dte;
        private readonly Lazy<ILogWatcherSettings> _lazyLogWatcherSettings;
        private readonly IEnumerable<ILogFileWatcher> _logWatchers;
        private readonly BlinkStickManager _blinkStickManager;
        private OleMenuCommand _openErrorLogCommand;
        private CommandBar _orchardLogWatcherToolbar;
        private bool _hasSeenErrorLogUpdate;
        private bool _errorIndicatorStateChanged;
        private ILogFileStatus _latestUpdatedLogFileStatus;


        private OpenErrorLogCommand(Package package)
        {
            _serviceProvider = package;

            _dte = Package.GetGlobalService(typeof(SDTE)) as DTE;
            _menuCommandService = _serviceProvider.GetService<IMenuCommandService>();
            _lazyLogWatcherSettings = new Lazy<ILogWatcherSettings>(
                _serviceProvider.GetService<ILogWatcherSettingsAccessor>().GetSettings);
            _logWatchers = _serviceProvider.GetServices<ILogFileWatcher>();
            _blinkStickManager = new BlinkStickManager();

            Initialize();
        }


        public static OpenErrorLogCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = Instance ?? new OpenErrorLogCommand(package);
        }

        public void Dispose()
        {
            _blinkStickManager.Dispose();

            foreach (var watcher in _logWatchers)
            {
                watcher.LogUpdated -= LogFileUpdatedCallback;
                watcher.Dispose();
            }

            _lazyLogWatcherSettings.Value.SettingsUpdated -= LogWatcherSettingsUpdatedCallback;
        }


        private void Initialize()
        {
            _hasSeenErrorLogUpdate = true;
            _errorIndicatorStateChanged = true;

            foreach (var watcher in _logWatchers)
            {
                watcher.LogUpdated += LogFileUpdatedCallback;
            }

            _lazyLogWatcherSettings.Value.SettingsUpdated += LogWatcherSettingsUpdatedCallback;

            _openErrorLogCommand = new OleMenuCommand(OpenErrorLogCallback, new CommandID(CommandSet, CommandId));
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

        private void OpenErrorLogCommandBeforeQueryStatusCallback(object sender, EventArgs e) =>
            UpdateOpenErrorLogCommandAccessibilityAndText();

        private void LogFileUpdatedCallback(object sender, LogChangedEventArgs context)
        {
            _hasSeenErrorLogUpdate = !context.LogFileStatus.HasContent;
            _latestUpdatedLogFileStatus = context.LogFileStatus;

            UpdateOpenErrorLogCommandAccessibilityAndText(context.LogFileStatus);
        }

        private void OpenErrorLogCallback(object sender, EventArgs e)
        {
            _hasSeenErrorLogUpdate = true;

            if (_latestUpdatedLogFileStatus != null && File.Exists(_latestUpdatedLogFileStatus.Path))
            {
                System.Diagnostics.Process.Start(_latestUpdatedLogFileStatus.Path);
            }
            else
            {
                DialogHelpers.Error("The log file doesn't exist.", "Open Orchard Error Log");
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
            var logWatcherSettings = _lazyLogWatcherSettings.Value;

            if (!_dte.Solution.IsOpen)
            {
                _openErrorLogCommand.Enabled = false;
                _openErrorLogCommand.Text = "Solution is not open";
            }
            else if (logWatcherSettings.LogWatcherEnabled &&
                ((logFileStatus?.HasContent ?? false) ||
                !_hasSeenErrorLogUpdate))
            {
                _openErrorLogCommand.Enabled = true;
                _openErrorLogCommand.Text = "Open Orchard error log";

                if (_errorIndicatorStateChanged)
                {
                    if (logWatcherSettings.BlinkBlinkStick) _blinkStickManager.Blink(logWatcherSettings.BlinkStickColor);
                    else _blinkStickManager.TurnOn(logWatcherSettings.BlinkStickColor);
                    _errorIndicatorStateChanged = false;
                }
            }
            else
            {
                _openErrorLogCommand.Enabled = false;
                _openErrorLogCommand.Text = "Orchard error log doesn't exist or hasn't been updated";
                _blinkStickManager.TurnOff();
                _errorIndicatorStateChanged = true;
            }
        }

        private void StartLogFileWatching()
        {
            foreach (var watcher in _logWatchers)
            {
                watcher.StartWatching();
            }
        }

        private void StopLogFileWatching()
        {
            foreach (var watcher in _logWatchers)
            {
                watcher.StopWatching();
            }
        }
    }
}
