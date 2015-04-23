using EnvDTE;
using EnvDTE80;
using Lombiq.VisualStudioExtensions.Forms;
using Lombiq.VisualStudioExtensions.Helpers;
using Lombiq.VisualStudioExtensions.Models;
using Lombiq.VisualStudioExtensions.Services;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lombiq.VisualStudioExtensions
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidLombiq_VisualStudioExtensionsPkgString)]
    public sealed class LombiqVisualStudioExtensionsPackage : Package
    {
        private readonly IOrchardProjectGenerator _orchardProjectGenerator;
        private readonly IDependencyToConstructorInjector _dependencyToConstructorInjector;
        private readonly DTE _dte;


        public LombiqVisualStudioExtensionsPackage()
        {
            _orchardProjectGenerator = new OrchardProjectGenerator();
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

                menuCommandService.AddCommand(
                    new MenuCommand(
                        AddModuleCallback,
                        new CommandID(GuidList.guidLombiq_VisualStudioExtensionsCmdSet, (int)PkgCmdIDList.cmdidAddModule)));
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

        private void AddModuleCallback(object sender, EventArgs e)
        {
            var vsProject = (from Project p in _dte.Solution.Projects where p.Name == GetActiveItemName() select p).FirstOrDefault();
            if (vsProject == null || (vsProject.Object as SolutionFolder) == null)
            {
                DialogHelpers.Error("Select a solution folder first.", "Add Orchard Module");

                return;
            }
            var selectedSolutionFolder = vsProject.Object as SolutionFolder;

            //var moduleTemplatePath = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "OrchardTemplates", "__BlankModule", "BlankModule.csproj");
            //For testing purposes.
            var moduleTemplatePath = Path.Combine(@"D:\OrchardTemplates", "__BlankModule", "BlankModule.csproj");
            if (!File.Exists(moduleTemplatePath))
            {
                DialogHelpers.Warning("Orchard module template was not found.", "Add Orchard Module");

                return;
            }

            using (var addModuleDialog = new AddModuleDialog())
            {
                if (addModuleDialog.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(addModuleDialog.ProjectName))
                    {
                        DialogHelpers.Warning("Module name cannot be empty.", "Add Orchard Module");

                        return;
                    }

                    var result = _orchardProjectGenerator.GenerateModule(
                        new CreateProjectContext 
                        { 
                            ProjectName = addModuleDialog.ProjectName, 
                            Solution = _dte.Solution,
                            TemplatePath = moduleTemplatePath,
                            SolutionFolder = selectedSolutionFolder
                        });

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
            MessageBox.Show(GetActiveItemName(), "Teszt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private string GetActiveItemName()
        {
            IntPtr hierarchyPtr, selectionContainerPtr;
            uint projectItemId;
            IVsMultiItemSelect mis;
            IVsMonitorSelection monitorSelection = (IVsMonitorSelection)GetGlobalService(typeof(SVsShellMonitorSelection));
            monitorSelection.GetCurrentSelection(out hierarchyPtr, out projectItemId, out mis, out selectionContainerPtr);

            var hierarchy = Marshal.GetTypedObjectForIUnknown(hierarchyPtr, typeof(IVsHierarchy)) as IVsHierarchy;
            if (hierarchy != null)
            {
                object value;
                hierarchy.GetProperty(projectItemId, (int)__VSHPROPID.VSHPROPID_Name, out value);

                return value.ToString();
            }

            return "";
        }
    }
}
