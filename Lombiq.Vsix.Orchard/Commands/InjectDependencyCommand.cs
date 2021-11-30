using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Forms;
using Lombiq.Vsix.Orchard.Helpers;
using Lombiq.Vsix.Orchard.Services.DependencyInjector;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;

namespace Lombiq.Vsix.Orchard.Commands
{
    internal sealed class InjectDependencyCommand
    {
        public const int CommandId = CommandIds.InjectDependencyCommandId;
        public static readonly Guid CommandSet = PackageGuids.LombiqOrchardVisualStudioExtensionCommandSetGuid;

        private readonly AsyncPackage _package;
        private readonly IDependencyInjector _dependencyInjector;
        private readonly IEnumerable<IFieldNameFromDependencyGenerator> _fieldNameGenerators;
        private readonly IEnumerable<IDependencyNameProvider> _dependencyNameProviders;

        public static InjectDependencyCommand Instance { get; private set; }

        private InjectDependencyCommand(
            AsyncPackage package,
            IDependencyInjector dependencyInjector,
            IEnumerable<IFieldNameFromDependencyGenerator> fieldNameGenerators,
            IEnumerable<IDependencyNameProvider> dependencyNameProviders)
        {
            _package = package;
            _dependencyInjector = dependencyInjector;
            _fieldNameGenerators = fieldNameGenerators;
            _dependencyNameProviders = dependencyNameProviders;
        }

        public static async Task CreateAsync(AsyncPackage package) => Instance = Instance ?? new InjectDependencyCommand(
                package,
                await package.GetServiceAsync<IDependencyInjector>().ConfigureAwait(true),
                await package.GetServicesAsync<IFieldNameFromDependencyGenerator>().ConfigureAwait(true),
                await package.GetServicesAsync<IDependencyNameProvider>().ConfigureAwait(true));

        public async Task InitializeUIAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            (await _package.GetServiceAsync<IMenuCommandService>().ConfigureAwait(true)).AddCommand(
                new MenuCommand(
                    MenuItemCallback,
                    new CommandID(CommandSet, CommandId)));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Usage",
            "VSTHRD102:Implement internal logic asynchronously",
            Justification = "The event handler must return void. The JoinableTaskFactory.Run is required to run the tasks asynchronously.")]
        private void MenuItemCallback(object sender, EventArgs e) =>
            ThreadHelper.JoinableTaskFactory.Run(MenuItemCallbackAsync);

        private async Task MenuItemCallbackAsync()
        {
            const string injectDependencyCaption = "Inject Dependency";
            var dte = await _package.GetDteAsync().ConfigureAwait(true);

            if (dte.ActiveDocument == null)
            {
                DialogHelpers.Error("Open a code file first.", injectDependencyCaption);

                return;
            }

            using (var injectDependencyDialog = new InjectDependencyDialog(
                _fieldNameGenerators,
                _dependencyNameProviders,
                _dependencyInjector.GetExpectedClassName(dte.ActiveDocument)))
            {
                if (injectDependencyDialog.ShowDialog() == DialogResult.OK)
                {
                    var dependencyInjectionData = injectDependencyDialog.DependencyInjectionData;

                    if (string.IsNullOrEmpty(dependencyInjectionData.FieldName) ||
                        string.IsNullOrEmpty(dependencyInjectionData.FieldType) ||
                        string.IsNullOrEmpty(dependencyInjectionData.ConstructorParameterName) ||
                        string.IsNullOrEmpty(dependencyInjectionData.ConstructorParameterType))
                    {
                        DialogHelpers.Warning("Field and constructor parameter names and types must be filled.", injectDependencyCaption);

                        return;
                    }

                    var result = _dependencyInjector.Inject(
                        dte.ActiveDocument,
                        injectDependencyDialog.DependencyInjectionData);

                    if (!result.Success)
                    {
                        DialogHelpers.Warning(
                            result.ErrorCode == DependencyInjectorErrorCodes.ClassNotFound
                                ? "Could not inject dependency because the class was not found in this file."
                                : "Could not inject dependency.",
                            injectDependencyCaption);
                    }
                }
            }
        }
    }
}
