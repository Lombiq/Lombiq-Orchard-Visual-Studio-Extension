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
using Task = System.Threading.Tasks.Task;

namespace Lombiq.Vsix.Orchard
{
    [ProvideService(typeof(IDependencyInjector), IsAsyncQueryable = true)]
    [ProvideService(typeof(IFieldNameFromDependencyGenerator), IsAsyncQueryable = true)]
    [ProvideService(typeof(ILogWatcherSettingsAccessor), IsAsyncQueryable = true)]
    [ProvideService(typeof(ILogFileWatcher), IsAsyncQueryable = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#LombiqOrchardVisualStudioExtensionName", "#LombiqOrchardVisualStudioExtensionDescription", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(LogWatcherOptionsPage), "Lombiq Orchard Visual Studio Extension", "Orchard Log Watcher", 120, 121, true)]
    [Guid(PackageGuids.LombiqOrchardVisualStudioExtensionPackageGuidString)]
    public sealed class LombiqOrchardVisualStudioExtensionPackage : AsyncPackage, ILogWatcherSettingsAccessor
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

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
