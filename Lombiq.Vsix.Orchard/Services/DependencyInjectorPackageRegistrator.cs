using EnvDTE;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Forms;
using Lombiq.Vsix.Orchard.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Lombiq.Vsix.Orchard.Services
{
    public class DependencyInjectorPackageRegistrator : IPackageRegistrator
    {
        private readonly IDependencyInjector _dependencyInjector;
        private readonly IEnumerable<IFieldNameFromDependencyGenerator> _fieldNameGenerators;


        public DependencyInjectorPackageRegistrator(
            IDependencyInjector dependencyInjector,
            IEnumerable<IFieldNameFromDependencyGenerator> fieldNameGenerators)
        {
            _dependencyInjector = dependencyInjector;
            _fieldNameGenerators = fieldNameGenerators;
        }


        public void RegisterCommands(DTE dte, IMenuCommandService menuCommandService)
        {
            // Initialize "Inject Dependency" menu item.
            var injectDependencyCallback = new EventHandler((sender, e) =>
            {
                var injectDependencyCaption = "Inject Dependency";

                if (dte.ActiveDocument == null)
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

                        var result = _dependencyInjector.Inject(dte.ActiveDocument, injectDependencyDialog.DependencyName, injectDependencyDialog.PrivateFieldName);

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
            });

            menuCommandService.AddCommand(
                new MenuCommand(
                    injectDependencyCallback,
                    new CommandID(PackageGuids.LombiqOrchardVisualStudioExtensionCommandSetGuid, (int)CommandIds.InjectDependencyCommandId)));
        }

        public void Dispose() { }
    }
}
