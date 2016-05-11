using EnvDTE;
using Lombiq.VisualStudioExtensions.Forms;
using Lombiq.VisualStudioExtensions.Helpers;
using Lombiq.VisualStudioExtensions.Services;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lombiq.VisualStudioExtensions
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.LombiqVisualStudioExtensionsPackageGuidString)]
    public sealed class LombiqVisualStudioExtensionsPackage : Package
    {
        private readonly IDependencyToConstructorInjector _dependencyToConstructorInjector;
        private readonly DTE _dte;


        public LombiqVisualStudioExtensionsPackage()
        {
            _dependencyToConstructorInjector = new DependecyToConstructorInjector();
            _dte = Package.GetGlobalService(typeof(SDTE)) as DTE;
        }


        protected override void Initialize()
        {
            base.Initialize();

            var menuCommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (menuCommandService != null)
            {
                menuCommandService.AddCommand(
                    new MenuCommand(
                        InjectDependencyCallback,
                        new CommandID(GuidList.LombiqVisualStudioExtensionsCommandSetGuid, (int)PkgCmdIDList.cmdidInjectDependency)));
            }
        }


        private void InjectDependencyCallback(object sender, EventArgs e)
        {
            if (_dte.ActiveDocument == null)
            {
                DialogHelpers.Error("Open a code file first.", "Inject Dependency");

                return;
            }

            using (var injectDependencyDialog = new InjectDependencyDialog())
            {
                if (injectDependencyDialog.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(injectDependencyDialog.DependencyName))
                    {
                        DialogHelpers.Warning("Dependency name cannot be empty.", "Inject Dependency");

                        return;
                    }

                    if (string.IsNullOrEmpty(injectDependencyDialog.PrivateFieldName))
                    {
                        DialogHelpers.Warning("Private field name cannot be empty.", "Inject Dependency");

                        return;
                    }

                    var result = _dependencyToConstructorInjector.Inject(_dte.ActiveDocument, injectDependencyDialog.DependencyName, injectDependencyDialog.PrivateFieldName);

                    if (!result.Success)
                    {
                        DialogHelpers.Warning(result.ErrorMessage);
                    }
                }
            }
        }
    }
}
