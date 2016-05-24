using EnvDTE;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Forms;
using Lombiq.Vsix.Orchard.Helpers;
using Lombiq.Vsix.Orchard.Services;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lombiq.Vsix.Orchard
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.LombiqOrchardVisualStudioExtensionPackageGuidString)]
    public sealed class LombiqOrchardVisualStudioExtensionPackage : Package
    {
        private readonly IDependencyInjector _dependencyInjector;
        private readonly DTE _dte;


        public LombiqOrchardVisualStudioExtensionPackage()
        {
            _dependencyInjector = new DependencyInjector();
            _dte = Package.GetGlobalService(typeof(SDTE)) as DTE;
        }


        protected override void Initialize()
        {
            base.Initialize();

            // Initialize "Inject Dependency" menu item.
            var menuCommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (menuCommandService != null)
            {
                menuCommandService.AddCommand(
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

            using (var injectDependencyDialog = new InjectDependencyDialog())
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
                            case DependencyInjectorErrorCodes.ConstructorNotFound:
                                DialogHelpers.Warning("Could not inject depencency because the constructor was not found.", injectDependencyCaption);
                                break;
                            default:
                                DialogHelpers.Warning("Could not inject dependency.", injectDependencyCaption);
                                break;
                        }
                    }
                }
            }
        }
    }
}
