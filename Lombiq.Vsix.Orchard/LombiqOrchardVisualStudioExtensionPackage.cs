using Lombiq.Vsix.Orchard.Commands;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Models;
using Lombiq.Vsix.Orchard.Options;
using Lombiq.Vsix.Orchard.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace Lombiq.Vsix.Orchard
{
    [ProvideService(typeof(IDependencyInjector))]
    [ProvideService(typeof(IFieldNameFromDependencyGenerator))]
    [ProvideService(typeof(ILogWatcherSettingsAccessor))]
    [ProvideService(typeof(ILogFileWatcher))]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#LombiqOrchardVisualStudioExtensionName", "#LombiqOrchardVisualStudioExtensionDescription", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(LogWatcherOptionsPage), "Lombiq Orchard Visual Studio Extension", "Orchard Log Watcher", 120, 121, true)]
    [Guid(PackageGuids.LombiqOrchardVisualStudioExtensionPackageGuidString)]
    public sealed class LombiqOrchardVisualStudioExtensionPackage : Package, ILogWatcherSettingsAccessor
    {
        protected override void Initialize()
        {
            base.Initialize();

            RegisterServices();

            InjectDependencyCommand.Initialize(this);
            OpenErrorLogCommand.Initialize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                OpenErrorLogCommand.Instance?.Dispose();
            }

            base.Dispose(disposing);
        }


        private void RegisterServices()
        {
            var serviceContainer = (IServiceContainer)this;

            serviceContainer.AddService<IDependencyInjector>(new DependencyInjector());
            serviceContainer.AddServices<IFieldNameFromDependencyGenerator>(
                new DefaultFieldNameFromDependencyGenerator(),
                new DefaultFieldNameFromGenericTypeGenerator(),
                new FieldNameFromIEnumerableGenerator());
            serviceContainer.AddService<ILogWatcherSettingsAccessor>(this);
            serviceContainer.AddServices<ILogFileWatcher>(
                new OrchardErrorLogFileWatcher(this),
                new OrchardCoreLogFileWatcher(this));
        }


        #region ILogWatcherSettings Members

        ILogWatcherSettings ILogWatcherSettingsAccessor.GetSettings() =>
            (ILogWatcherSettings)GetDialogPage(typeof(LogWatcherOptionsPage));

        #endregion
    }
}
