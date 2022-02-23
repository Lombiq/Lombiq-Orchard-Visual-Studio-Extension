using Microsoft.VisualStudio.Shell;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    public sealed class OrchardErrorLogFileWatcher : LogFileWatcherBase
    {
        protected override Task<string> GetLogFileNameAsync() =>
            Task.FromResult(
                $"orchard-error-{DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.log");

        public OrchardErrorLogFileWatcher(AsyncPackage package, ILogWatcherSettingsAccessor logWatcherSettingsAccessor)
            : base(package, logWatcherSettingsAccessor)
        { }
    }
}
