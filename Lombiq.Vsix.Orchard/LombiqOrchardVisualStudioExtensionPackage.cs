using EnvDTE;
using Lombiq.Vsix.Orchard.Commands;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Models;
using Lombiq.Vsix.Orchard.Options;
using Lombiq.Vsix.Orchard.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace Lombiq.Vsix.Orchard
{
    [ProvideService(typeof(IDependencyInjector))]
    [ProvideService(typeof(IFieldNameFromDependencyGenerator))]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#LombiqOrchardVisualStudioExtensionName", "#LombiqOrchardVisualStudioExtensionDescription", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(LogWatcherOptionsPage), "Lombiq Orchard Visual Studio Extension", "Orchard Log Watcher", 120, 121, true)]
    [Guid(PackageGuids.LombiqOrchardVisualStudioExtensionPackageGuidString)]
    public sealed class LombiqOrchardVisualStudioExtensionPackage : Package, ILogWatcherSettingsAccessor
    {
        private readonly DTE _dte;
        private readonly IMenuCommandService _menuCommandService;
        private readonly IEnumerable<IPackageRegistrator> _packageRegistrators;


        public LombiqOrchardVisualStudioExtensionPackage()
        {
            _dte = GetGlobalService(typeof(SDTE)) as DTE;
            _menuCommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            var fieldNameGenerators = new IFieldNameFromDependencyGenerator[]
            {
                new DefaultFieldNameFromDependencyGenerator(),
                new DefaultFieldNameFromGenericTypeGenerator(),
                new FieldNameFromIEnumerableGenerator()
            };

            var logWatcherSettingsAccessor = this;
            var logWatcher = new OrchardErrorLogFileWatcher(logWatcherSettingsAccessor, _dte);

            var logWatcherPackageRegistrator = new LogWatcherPackageRegistrator(
                logWatcherSettingsAccessor,
                logWatcher);

            _packageRegistrators = new IPackageRegistrator[]
                {
                    logWatcherPackageRegistrator
                };
        }


        protected override void Initialize()
        {
            base.Initialize();

            RegisterServices();

            InjectDependencyCommand.Initialize(this);

            foreach (var registrator in _packageRegistrators)
            {
                registrator.RegisterCommands(_dte, _menuCommandService);
            }
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var registrator in _packageRegistrators)
            {
                registrator.Dispose();
            }

            base.Dispose(disposing);
        }


        private void RegisterServices()
        {
            var serviceContainer = (IServiceContainer)this;

            serviceContainer.AddService(typeof(IDependencyInjector), new DependencyInjector());
            serviceContainer.AddService(typeof(IEnumerable<IFieldNameFromDependencyGenerator>),
                new IFieldNameFromDependencyGenerator[]
                {
                    new DefaultFieldNameFromDependencyGenerator(),
                    new DefaultFieldNameFromGenericTypeGenerator(),
                    new FieldNameFromIEnumerableGenerator()
                });
        }


        #region ILogWatcherSettings Members

        ILogWatcherSettings ILogWatcherSettingsAccessor.GetSettings() =>
            (ILogWatcherSettings)GetDialogPage(typeof(LogWatcherOptionsPage));

        #endregion
    }
}
