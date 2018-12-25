using System;

namespace Lombiq.Vsix.Orchard.Services
{
    public class OrchardCoreLogFileWatcher : LogFileWatcherBase, ILogFileWatcher
    {
        protected override string GetLogFileName() => "orchard-log-" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";


        public OrchardCoreLogFileWatcher(IServiceProvider serviceProvider) : base(serviceProvider) { }
    }
}
