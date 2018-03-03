using System;

namespace Lombiq.Vsix.Orchard.Services
{
    public class OrchardErrorLogFileWatcher : LogFileWatcherBase, ILogFileWatcher
    {
        protected override string GetLogFileName() => "orchard-error-" + DateTime.Today.ToString("yyyy.MM.dd") + ".log";


        public OrchardErrorLogFileWatcher(IServiceProvider serviceProvider) : base(serviceProvider) { }
    }
}
