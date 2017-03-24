using EnvDTE;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Forms;
using Lombiq.Vsix.Orchard.Helpers;
using Lombiq.Vsix.Orchard.Options;
using Lombiq.Vsix.Orchard.Services;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Lombiq.Vsix.Orchard.Models;
using Microsoft.VisualStudio;

namespace Lombiq.Vsix.Orchard
{
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(LogWatcherOptionsPage), "Lombiq Orchard Visual Studio Extension", "General", 120, 121, true)]
    [Guid(PackageGuids.LombiqOrchardVisualStudioExtensionPackageGuidString)]
    public sealed class LombiqOrchardVisualStudioExtensionPackage : Package, ILogWatcherSettingsAccessor
    {
        private readonly IDependencyInjector _dependencyInjector;
        private readonly IEnumerable<IFieldNameFromDependencyGenerator> _fieldNameGenerators;
        private readonly DTE _dte;
        private readonly ILogFileWatcher _logWatcher;
        private OleMenuCommand _openErrorLogCommand;


        public LombiqOrchardVisualStudioExtensionPackage()
        {
            _dependencyInjector = new DependencyInjector();
            _fieldNameGenerators = new IFieldNameFromDependencyGenerator[] 
            {
                new DefaultFieldNameFromDependencyGenerator(),
                new DefaultFieldNameFromGenericTypeGenerator(),
                new FieldNameFromIEnumerableGenerator()
            };
            _dte = Package.GetGlobalService(typeof(SDTE)) as DTE;
            _logWatcher = new OrchardErrorLogFileWatcher(this, _dte);
        }


        protected override void Initialize()
        {
            base.Initialize();
            
            _dte.Events.SolutionEvents.Opened += SolutionOpenedCallback;
            _dte.Events.SolutionEvents.AfterClosing += SolutionClosedCallback;
            _logWatcher.LogUpdated += LogFileUpdatedCallback;

            var menuCommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (menuCommandService != null)
            {
                // Initialize "Inject Dependency" menu item.
                menuCommandService.AddCommand(
                    new MenuCommand(
                        InjectDependencyCallback,
                        new CommandID(PackageGuids.LombiqOrchardVisualStudioExtensionCommandSetGuid, (int)CommandIds.InjectDependencyCommandId)));

                // Initialize "Open Error Log" toolbar button.
                _openErrorLogCommand = new OleMenuCommand(
                    OpenErrorLogCallback, 
                    new CommandID(
                        PackageGuids.LombiqOrchardVisualStudioExtensionCommandSetGuid, 
                        (int)CommandIds.OpenErrorLogCommandId));

                menuCommandService.AddCommand(_openErrorLogCommand);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _dte.Events.SolutionEvents.Opened -= SolutionOpenedCallback;
            _dte.Events.SolutionEvents.AfterClosing -= SolutionClosedCallback;
            _logWatcher.LogUpdated -= LogFileUpdatedCallback;

            _logWatcher.Dispose();

            base.Dispose(disposing);
        }


        private void LogFileUpdatedCallback(object sender, ILogChangedContext context)
        {
            UpdateOpenErrorLogCommandAccessibility(context);
        }

        private void SolutionOpenedCallback()
        {
            _logWatcher.StartWatching();

            UpdateOpenErrorLogCommandAccessibility();
        }

        private void SolutionClosedCallback()
        {
            _logWatcher.StopWatching();

            UpdateOpenErrorLogCommandAccessibility();
        }

        private void OpenErrorLogCallback(object sender, EventArgs e)
        {
            DialogHelpers.Information("Clicked");
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

        private void UpdateOpenErrorLogCommandAccessibility(ILogChangedContext context = null)
        {
            _openErrorLogCommand.Enabled = _dte.Solution.IsOpen && 
                (context != null ? 
                    context.HasContent : 
                    _logWatcher.HasContent());
        }

        #region ILogWatcherSettings Members

        ILogWatcherSettings ILogWatcherSettingsAccessor.GetSettings()
        {
            return (ILogWatcherSettings)GetDialogPage(typeof(LogWatcherOptionsPage));
        }

        #endregion
    }
}
