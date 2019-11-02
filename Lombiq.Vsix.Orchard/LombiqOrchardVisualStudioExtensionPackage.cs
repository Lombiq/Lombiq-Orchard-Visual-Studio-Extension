using Lombiq.Vsix.Orchard.Commands;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Models;
using Lombiq.Vsix.Orchard.Options;
using Lombiq.Vsix.Orchard.Services.DependencyInjector;
using Lombiq.Vsix.Orchard.Services.LogWatcher;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;

namespace Lombiq.Vsix.Orchard
{
    [ProvideService(typeof(IDependencyInjector), IsAsyncQueryable = true)]
    [ProvideService(typeof(IFieldNameFromDependencyGenerator), IsAsyncQueryable = true)]
    [ProvideService(typeof(ILogWatcherSettingsAccessor), IsAsyncQueryable = true)]
    [ProvideService(typeof(ILogFileWatcher), IsAsyncQueryable = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(
        // Such values can supposedly come from resx files (see: https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-an-extension-with-a-vspackage?view=vs-2019)
        // but that code doesn't work.
        "Lombiq Orchard Visual Studio Extension", 
        "Visual Studio extension with many features frequently used by Lombiq developers. Contains Orchard-related as well as generic goodies.",
        ExtensionVersion.Current,
        IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(LogWatcherOptionsPage), "Lombiq Orchard Visual Studio Extension", "Orchard Log Watcher", 120, 121, true)]
    [Guid(PackageGuids.LombiqOrchardVisualStudioExtensionPackageGuidString)]
    public sealed class LombiqOrchardVisualStudioExtensionPackage : AsyncPackage, ILogWatcherSettingsAccessor
    {
        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            RegisterServices();

            InjectDependencyCommand.Initialize(this);
            OpenErrorLogCommand.Initialize(this);

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
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
                new FieldNameFromIEnumerableGenerator(),
                new FieldNameFromLocalizerGenerator(),
                new SimplifiedFieldNameFromGenericTypeGenerator());
            serviceContainer.AddServices<IDependencyNameProvider>(
                new CommonDependencyNamesProvider());
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
