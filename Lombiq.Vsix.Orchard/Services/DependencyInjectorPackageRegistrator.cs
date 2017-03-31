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
    public class DependencyInjectorPackageRegistrator : PackageRegistratorBase
    {
        private readonly IDependencyInjector _dependencyInjector;
        private readonly IEnumerable<IFieldNameFromDependencyGenerator> _fieldNameGenerators;


        public DependencyInjectorPackageRegistrator(
            DTE dte,
            IMenuCommandService menuCommandService,
            IDependencyInjector dependencyInjector,
            IEnumerable<IFieldNameFromDependencyGenerator> fieldNameGenerators) :
            base(dte, menuCommandService)
        {
            _dependencyInjector = dependencyInjector;
            _fieldNameGenerators = fieldNameGenerators;
        }


        public override void RegisterCommands()
        {
            // Initialize "Inject Dependency" menu item.
            _menuCommandService?.AddCommand(
                new MenuCommand(
                    InjectDependencyCallback,
                    new CommandID(PackageGuids.LombiqOrchardVisualStudioExtensionCommandSetGuid, (int)CommandIds.InjectDependencyCommandId)));
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
    }
}
