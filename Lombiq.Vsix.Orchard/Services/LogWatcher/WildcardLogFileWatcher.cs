using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    public sealed class WildcardLogFileWatcher : LogFileWatcherBase
    {
        protected override async Task<string> GetLogFileNameAsync() =>
            (await _logWatcherSettingsAccessor.GetSettingsAsync()).LogFileNameSearchPattern.Trim();

        public WildcardLogFileWatcher(AsyncPackage package, ILogWatcherSettingsAccessor logWatcherSettingsAccessor) :
            base(package, logWatcherSettingsAccessor)
        { }

        protected override IEnumerable<string> GetAllMatchingPaths(string root, IEnumerable<string> patterns, string logFileName)
        {
            var allMatchingFiles = base.GetAllMatchingPaths(root, patterns, logFileName);

            return allMatchingFiles.Skip(1).Any() ?
                allMatchingFiles.OrderByDescending(path => File.GetLastWriteTime(path)).Take(1) :
                allMatchingFiles;
        }
    }
}
