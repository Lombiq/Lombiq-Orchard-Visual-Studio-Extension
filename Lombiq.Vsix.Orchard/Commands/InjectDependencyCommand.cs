using EnvDTE;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Forms;
using Lombiq.Vsix.Orchard.Helpers;
using Lombiq.Vsix.Orchard.Services;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;

namespace Lombiq.Vsix.Orchard.Commands
{
    internal sealed class InjectDependencyCommand
    {
        public const int CommandId = CommandIds.InjectDependencyCommandId;
        public static readonly Guid CommandSet = PackageGuids.LombiqOrchardVisualStudioExtensionCommandSetGuid;


        private readonly Package _package;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMenuCommandService _menuCommandService;
        private readonly IDependencyInjector _dependencyInjector;
        private readonly IEnumerable<IFieldNameFromDependencyGenerator> _fieldNameGenerators;
        private readonly DTE _dte;

        
        private InjectDependencyCommand(Package package)
        {
            _package = package;
            _serviceProvider = package;

            _dte = Package.GetGlobalService(typeof(SDTE)) as DTE;
            _dependencyInjector = _serviceProvider.GetService<IDependencyInjector>();
            _fieldNameGenerators = _serviceProvider.GetServices<IFieldNameFromDependencyGenerator>();
            _menuCommandService = _serviceProvider.GetService<IMenuCommandService>();

            Initialize();
        }


        public static InjectDependencyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = Instance ?? new InjectDependencyCommand(package);
        }


        private void Initialize()
        {
            _menuCommandService.AddCommand(
                new MenuCommand(
                    MenuItemCallback,
                    new CommandID(CommandSet, CommandId)));
        }

        private void MenuItemCallback(object sender, EventArgs e)
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
