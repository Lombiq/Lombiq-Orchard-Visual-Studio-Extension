using System;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    public sealed class OrchardCoreLogFileWatcher : LogFileWatcherBase
    {
        protected override string GetLogFileName() => "orchard-log-" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";


        public OrchardCoreLogFileWatcher(IServiceProvider serviceProvider) : base(serviceProvider) { }
    }
}
