using Lombiq.Vsix.Orchard.Commands;
using Lombiq.Vsix.Orchard.Constants;
using Lombiq.Vsix.Orchard.Models;
using Lombiq.Vsix.Orchard.Options;
using Lombiq.Vsix.Orchard.Services.DependencyInjector;
using Lombiq.Vsix.Orchard.Services.LogWatcher;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace Lombiq.Vsix.Orchard
{
    [ProvideService(typeof(IDependencyInjector), IsAsyncQueryable = true)]
    [ProvideService(typeof(IFieldNameFromDependencyGenerator), IsAsyncQueryable = true)]
    [ProvideService(typeof(IDependencyNameProvider), IsAsyncQueryable = true)]
    [ProvideService(typeof(ILogFileWatcher), IsAsyncQueryable = true)]
    [ProvideService(typeof(IBlinkStickManager), IsAsyncQueryable = true)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(
        // Such values can supposedly come from resx files (see:
        // https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-an-extension-with-a-vspackage?view=vs-2019)
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
        ILogWatcherSettings ILogWatcherSettingsAccessor.GetSettings() =>
            (ILogWatcherSettings)GetDialogPage(typeof(LogWatcherOptionsPage));


        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // On using AsyncPackage see:
            // https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-provide-an-asynchronous-visual-studio-service
            // https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-use-asyncpackage-to-load-vspackages-in-the-background

            RegisterServices();

            await InjectDependencyCommand.Create(this);
            await OpenErrorLogCommand.Create(this, this);

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await InjectDependencyCommand.Instance.InitializeUI();
            await OpenErrorLogCommand.Instance.InitializeUI();
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
            // Note that all dependencies need to be added with IsAsyncQueryable = true above in attributes.
            // The current object can't be registered as an ILogWatcherSettingsAccessor dependency because it can't be
            // resolved in an async manner. So just using it directly.

            this.AddService<IDependencyInjector, DependencyInjector>();

            this.AddService<IFieldNameFromDependencyGenerator>(() => Task.FromResult((object)new IFieldNameFromDependencyGenerator[]
            {
                new DefaultFieldNameFromDependencyGenerator(),
                new DefaultFieldNameFromGenericTypeGenerator(),
                new FieldNameFromIEnumerableGenerator(),
                new FieldNameFromLocalizerGenerator(),
                new SimplifiedFieldNameFromGenericTypeGenerator()
            }));

            this.AddService<IDependencyNameProvider>(() => Task.FromResult((object)new IDependencyNameProvider[]
            {
                new CommonDependencyNamesProvider()
            }));

            this.AddService<ILogFileWatcher>(() =>
            {
                var dte = this.GetDte();

                return Task.FromResult((object)new ILogFileWatcher[]
                {
                    new OrchardErrorLogFileWatcher(this, dte),
                    new OrchardCoreLogFileWatcher(this, dte),
                    new WildcardLogFileWatcher(this, dte)
                });
            });

            this.AddService<IBlinkStickManager, BlinkStickManager>();
        }
    }
}
