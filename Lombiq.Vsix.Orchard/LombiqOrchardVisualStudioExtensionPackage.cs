using EnvDTE;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Forms;
using Lombiq.Vsix.Orchard.Helpers;
using Lombiq.Vsix.Orchard.Models;
using Lombiq.Vsix.Orchard.Options;
using Lombiq.Vsix.Orchard.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lombiq.Vsix.Orchard
{
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(LogWatcherOptionsPage), "Orchard Log Watcher", "General", 120, 121, true)]
    [Guid(PackageGuids.LombiqOrchardVisualStudioExtensionPackageGuidString)]
    public sealed class LombiqOrchardVisualStudioExtensionPackage : Package, ILogWatcherSettingsAccessor
    {
        private readonly IDependencyInjector _dependencyInjector;
        private readonly IEnumerable<IFieldNameFromDependencyGenerator> _fieldNameGenerators;
        private readonly DTE _dte;
        private readonly IMenuCommandService _menuCommandService;
        private readonly ILogFileWatcher _logWatcher;
        private readonly ILogWatcherSettingsAccessor _logWatcherSettingsAccessor;
        private OleMenuCommand _openErrorLogCommand;
        private CommandBar _orchardLogWatcherToolbar;
        private bool _errorLogSeen;


        public LombiqOrchardVisualStudioExtensionPackage()
        {
            _dependencyInjector = new DependencyInjector();
            _fieldNameGenerators = new IFieldNameFromDependencyGenerator[]
            {
                new DefaultFieldNameFromDependencyGenerator(),
                new DefaultFieldNameFromGenericTypeGenerator(),
                new FieldNameFromIEnumerableGenerator()
            };
            _dte = GetGlobalService(typeof(SDTE)) as DTE;
            _menuCommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            _logWatcherSettingsAccessor = this;
            _logWatcher = new OrchardErrorLogFileWatcher(_logWatcherSettingsAccessor, _dte);
        }


        protected override void Initialize()
        {
            base.Initialize();

            InitializeDependencyInjector();
            InitializeLogWatcher();
        }

        protected override void Dispose(bool disposing)
        {
            DisposeLogWatcher();

            base.Dispose(disposing);
        }


        #region Log Watcher Helpers and Callbacks

        private void InitializeLogWatcher()
        {
            _logWatcher.LogUpdated += LogFileUpdatedCallback;
            GetLogWatcherSettings().SettingsUpdated += LogWatcherSettingsUpdatedCallback;

            if (GetLogWatcherSettings().LogWatcherEnabled)
            {
                StartLogFileWatching();
            }

            if (_menuCommandService != null)
            {
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
            }
        }
        
        private void DisposeLogWatcher()
        {
            _logWatcher.LogUpdated -= LogFileUpdatedCallback;
            GetLogWatcherSettings().SettingsUpdated -= LogWatcherSettingsUpdatedCallback;

            _logWatcher.Dispose();
        }

        private void OpenErrorLogCommandBeforeQueryStatusCallback(object sender, EventArgs e) =>
            UpdateOpenErrorLogCommandAccessibility();

        private void LogFileUpdatedCallback(object sender, LogChangedEventArgs context)
        {
            _errorLogSeen = false;

            UpdateOpenErrorLogCommandAccessibility(context.LogFileStatus);
        }

        private void OpenErrorLogCallback(object sender, EventArgs e)
        {
            var status = GetLogFileStatus();

            if (status.Exists)
            {
                System.Diagnostics.Process.Start(status.FileName);
            }
            else
            {
                DialogHelpers.Error("The log file doesn't exists.", "Open Orchard Error Log");
            }

            _errorLogSeen = true;

            UpdateOpenErrorLogCommandAccessibility();
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

            UpdateOpenErrorLogCommandAccessibility();
        }

        private void UpdateOpenErrorLogCommandAccessibility(ILogFileStatus logFileStatus = null) =>
            _openErrorLogCommand.Enabled = !_errorLogSeen &&
                _dte.Solution.IsOpen &&
                GetLogWatcherSettings().LogWatcherEnabled &&
                (logFileStatus ?? GetLogFileStatus()).HasContent;

        private ILogWatcherSettings GetLogWatcherSettings() =>
            _logWatcherSettingsAccessor.GetSettings();

        private ILogFileStatus GetLogFileStatus() =>
            _logWatcher.GetLogFileStatus();

        private void StartLogFileWatching() =>
            _logWatcher.StartWatching();

        private void StopLogFileWatching() =>
            _logWatcher.StopWatching();

        #endregion

        #region Dependency Injector Helpers and Callbacks

        private void InitializeDependencyInjector()
        {
            if (_menuCommandService != null)
            {
                // Initialize "Inject Dependency" menu item.
                _menuCommandService.AddCommand(
                    new MenuCommand(
                        InjectDependencyCallback,
                        new CommandID(PackageGuids.LombiqOrchardVisualStudioExtensionCommandSetGuid, (int)CommandIds.InjectDependencyCommandId)));
            }
        }

        private void InjectDependencyCallback(object sender, EventArgs e)
        {
            var injectDependencyCaption = "Inject Dependency";

            if (_dte.ActiveDocument == null)
            {
                DialogHelpers.Error("Open a code file first.", injectDependencyCaption);

                return;
            }

            using (var injectDependencyDialog = new InjectDependencyDialog(_fieldNameGenerators))
            {
                if (injectDependencyDialog.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(injectDependencyDialog.DependencyName))
                    {
                        DialogHelpers.Warning("Dependency name cannot be empty.", injectDependencyCaption);

                        return;
                    }

                    if (string.IsNullOrEmpty(injectDependencyDialog.PrivateFieldName))
                    {
                        DialogHelpers.Warning("Private field name cannot be empty.", injectDependencyCaption);

                        return;
                    }

                    var result = _dependencyInjector.Inject(_dte.ActiveDocument, injectDependencyDialog.DependencyName, injectDependencyDialog.PrivateFieldName);

                    if (!result.Success)
                    {
                        switch (result.ErrorCode)
                        {
                            case DependencyInjectorErrorCodes.ClassNotFound:
                                DialogHelpers.Warning("Could not inject depencency because the class was not found in this file.", injectDependencyCaption);
                                break;
                            default:
                                DialogHelpers.Warning("Could not inject dependency.", injectDependencyCaption);
                                break;
                        }
                    }
                }
            }
        }

        #endregion

        #region ILogWatcherSettings Members

        ILogWatcherSettings ILogWatcherSettingsAccessor.GetSettings() => 
            (ILogWatcherSettings)GetDialogPage(typeof(LogWatcherOptionsPage));

        #endregion
    }
}
