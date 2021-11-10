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
using System.Threading.Tasks;
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
    // Such values can supposedly come from resx files (see:
    // https://docs.microsoft.com/en-us/visualstudio/extensibility/creating-an-extension-with-a-vspackage?view=vs-2019)
    // but that code doesn't work.
    [InstalledProductRegistration(
        "Lombiq Orchard Visual Studio Extension",
        "Visual Studio extension with many features frequently used by Lombiq developers. Contains Orchard-related as well as generic goodies.",
        ExtensionVersion.Current,
        IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(LogWatcherOptionsPage), "Lombiq Orchard Visual Studio Extension", "Orchard Log Watcher", 120, 121, true)]
    [Guid(PackageGuids.LombiqOrchardVisualStudioExtensionPackageGuidString)]
    public sealed class LombiqOrchardVisualStudioExtensionPackage : AsyncPackage, ILogWatcherSettingsAccessor
    {
        async Task<ILogWatcherSettings> ILogWatcherSettingsAccessor.GetSettingsAsync()
        {
            // The caller will magically resume on its original thread so we can safely switch to the UI thread here
            // (see:
            // https://devblogs.microsoft.com/premier-developer/asynchronous-and-multithreaded-programming-within-vs-using-the-joinabletaskfactory/
            // "The implementation of async methods you call (such as DoSomethingAsync or SaveWorkToDiskAsync) does not
            // impact the thread of the calling method.")
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            return (ILogWatcherSettings)GetDialogPage(typeof(LogWatcherOptionsPage));
        }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);

            // On using AsyncPackage see:
            // https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-provide-an-asynchronous-visual-studio-service
            // https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-use-asyncpackage-to-load-vspackages-in-the-background
            // Be sure to read
            // https://devblogs.microsoft.com/premier-developer/asynchronous-and-multithreaded-programming-within-vs-using-the-joinabletaskfactory/
            // for a lot of background info on how and why to use JoinableTaskFactory.

            // Here we need to take care of only doing the bare minimum on the UI thread, not to block it. However,
            // some initialization that needs to happen in the background still depends on data from the UI thread so
            // we need to switch back and forth.

            RegisterServices();

            await InjectDependencyCommand.CreateAsync(this);
            await OpenErrorLogCommand.CreateAsync(this, this);

            await InjectDependencyCommand.Instance.InitializeUIAsync();
            await OpenErrorLogCommand.Instance.InitializeUIAsync();
        }

        protected override void Dispose(bool disposing)
        {
            ThreadHelper.JoinableTaskFactory.Run(async () => await DisposeAsync(disposing));
            base.Dispose(disposing);
        }

        public static async Task DisposeAsync(bool disposing)
        {
            if (disposing && OpenErrorLogCommand.Instance != null)
            {
                await OpenErrorLogCommand.Instance.DisposeAsync();
            }
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
                new SimplifiedFieldNameFromGenericTypeGenerator(),
            }));

            this.AddService<IDependencyNameProvider>(() => Task.FromResult((object)new IDependencyNameProvider[]
            {
                new CommonDependencyNamesProvider(),
            }));

            this.AddService<ILogFileWatcher>(() => Task.FromResult((object)new ILogFileWatcher[]
                {
                    new OrchardErrorLogFileWatcher(this, this),
                    new OrchardCoreLogFileWatcher(this, this),
                    new WildcardLogFileWatcher(this, this),
                }));

            this.AddService<IBlinkStickManager, BlinkStickManager>();
        }
    }
}
