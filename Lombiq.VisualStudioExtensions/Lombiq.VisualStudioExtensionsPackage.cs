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
    [Guid(GuidList.guidLombiq_VisualStudioExtensionsPkgString)]
    public sealed class VisualStudioExtensionsPackage : Package
    {
        private readonly IDependencyToConstructorInjector _dependencyToConstructorInjector;
        private readonly DTE _dte;


        public VisualStudioExtensionsPackage()
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
                        new CommandID(GuidList.guidLombiq_VisualStudioExtensionsCmdSet, (int)PkgCmdIDList.cmdidInjectDependency)));

                menuCommandService.AddCommand(
                    new MenuCommand(
                        TestCallback,
                        new CommandID(GuidList.guidLombiq_VisualStudioExtensionsCmdSet, (int)PkgCmdIDList.cmdidTest)));
            }
        }


        private void InjectDependencyCallback(object sender, EventArgs e)
        {
            if (_dte.ActiveDocument == null)
            {
                MessageBox.Show("Open a code file first.", "Inject Dependency", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

                    var result = _dependencyToConstructorInjector.Inject(_dte.ActiveDocument, injectDependencyDialog.DependencyName);

                    if (!result.Success)
                    {
                        DialogHelpers.Warning(result.ErrorMessage);
                    }
                }
            }
        }

        // For testing purposes.
        private void TestCallback(object sender, EventArgs e)
        {
            MessageBox.Show("Hurrá!", "Teszt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
