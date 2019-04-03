using System;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    public sealed class OrchardErrorLogFileWatcher : LogFileWatcherBase
    {
        protected override string GetLogFileName() => "orchard-error-" + DateTime.Today.ToString("yyyy.MM.dd") + ".log";


        public OrchardErrorLogFileWatcher(IServiceProvider serviceProvider) : base(serviceProvider) { }
    }
}
