using EnvDTE;
using Lombiq.VisualStudioExtensions.Exceptions;
using Lombiq.VisualStudioExtensions.Forms;
using Lombiq.VisualStudioExtensions.Services;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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


        public VisualStudioExtensionsPackage()
        {
            _dependencyToConstructorInjector = new DependecyToConstructorInjector();
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
            var dte = Package.GetGlobalService(typeof(SDTE)) as DTE;

            if (dte.ActiveDocument == null)
            {
                MessageBox.Show("Open a code file first.", "Inject Dependency", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            var injectDependencyDialog = new InjectDependencyDialog();

            if (injectDependencyDialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(injectDependencyDialog.DependencyName))
                {
                    MessageBox.Show("Dependency name cannot be empty.", "Inject Dependency", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                try
                {
                    var fileName = dte.ActiveDocument.FullName;
                    var constructorName = Path.GetFileNameWithoutExtension(fileName);

                    var textDoc = dte.ActiveDocument.Object() as TextDocument;
                    EditPoint editPoint = (EditPoint)textDoc.StartPoint.CreateEditPoint();
                    EditPoint endPoint = (EditPoint)textDoc.EndPoint.CreateEditPoint();
                    var text = editPoint.GetText(endPoint);

                    var newCode = _dependencyToConstructorInjector.Inject(injectDependencyDialog.DependencyName, text, constructorName);

                    editPoint.ReplaceText(endPoint, newCode, 0);
                }
                catch (DependencyToConstructorInjectorException ex)
                {
                    MessageBox.Show(string.Format("Could not inject dependency. Error code: {0}", ex.ErrorCode), "Inject Dependency", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
