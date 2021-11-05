using Microsoft.VisualStudio.Shell;
using System;
using System.Threading.Tasks;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    public sealed class OrchardCoreLogFileWatcher : LogFileWatcherBase
    {
        protected override Task<string> GetLogFileNameAsync() =>
            System.Threading.Tasks.Task.FromResult("orchard-log-" + DateTime.Today.ToString("yyyy-MM-dd") + ".log");

        public OrchardCoreLogFileWatcher(AsyncPackage package, ILogWatcherSettingsAccessor logWatcherSettingsAccessor) :
            base(package, logWatcherSettingsAccessor)
        { }
    }
}
