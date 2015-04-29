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
    [ProvideOptionPageAttribute(typeof(LombiqVisualStudioExtensionsOptionPage), "Lombiq VS Extensions", "General", 113, 114, true, new string[] { "Lombiq VS Extensions Options" })]
    [Guid(GuidList.LombiqVisualStudioExtensionsPackageGuidString)]
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
                        new CommandID(GuidList.LombiqVisualStudioExtensionsCommandSetGuid, (int)PkgCmdIDList.cmdidInjectDependency)));

                menuCommandService.AddCommand(
                    new MenuCommand(
                        TestCallback,
                        new CommandID(GuidList.LombiqVisualStudioExtensionsCommandSetGuid, (int)PkgCmdIDList.cmdidTest)));

                menuCommandService.AddCommand(
                    new MenuCommand(
                        AddModuleCallback,
                        new CommandID(GuidList.LombiqVisualStudioExtensionsCommandSetGuid, (int)PkgCmdIDList.cmdidAddModule)));
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

        private void AddModuleCallback(object sender, EventArgs e)
        {
            var vsProject = (from Project p in _dte.Solution.Projects where p.Name == GetActiveItemName() select p).FirstOrDefault();
            if (vsProject == null || (vsProject.Object as SolutionFolder) == null)
            {
                DialogHelpers.Warning("Select a solution folder first.", "Add Orchard Module");

                return;
            }
            var selectedSolutionFolder = vsProject.Object as SolutionFolder;
            
            var optionPage = GetDialogPage(typeof(LombiqVisualStudioExtensionsOptionPage)) as LombiqVisualStudioExtensionsOptionPage;
            if (optionPage == null)
            {
                DialogHelpers.Error("Lombiq VS Extensions options are not accessible.", "Add Orchard Module");
            }
            if (string.IsNullOrEmpty(optionPage.OrchardModuleTemlatePath))
            {
                DialogHelpers.Warning("Please set Orchard module template path in the options first.", "Add Orchard Module");
            }
            var moduleTemplatePath = Path.Combine(optionPage.OrchardModuleTemlatePath, "ModuleTemplate.csproj");
            
            if (!File.Exists(moduleTemplatePath))
            {
                DialogHelpers.Warning("Orchard module template was not found.", "Add Orchard Module");

                return;
            }

            using (var addModuleDialog = new AddModuleDialog())
            {
                addModuleDialog.Website = optionPage.DefaultWebsite;
                addModuleDialog.Author = optionPage.DefaultAuthor;

                if (addModuleDialog.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(addModuleDialog.ProjectName))
                    {
                        DialogHelpers.Warning("Module name cannot be empty.", "Add Orchard Module");

                        return;
                    }

                    var result = _orchardProjectGenerator.GenerateModule(
                        new CreateModuleContext 
                        { 
                            ProjectName = addModuleDialog.ProjectName, 
                            Solution = _dte.Solution,
                            TemplatePath = moduleTemplatePath,
                            SolutionFolder = selectedSolutionFolder,
                            Author = addModuleDialog.Author,
                            Description = addModuleDialog.Description,
                            Website = addModuleDialog.Website
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
