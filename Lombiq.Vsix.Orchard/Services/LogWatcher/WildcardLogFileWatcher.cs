using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    public sealed class WildcardLogFileWatcher : LogFileWatcherBase
    {
        protected override string GetLogFileName() => _logWatcherSettingsAccessor.GetSettings().LogFileNameSearchPattern.Trim();


        public WildcardLogFileWatcher(IServiceProvider serviceProvider) : base(serviceProvider) { }


        protected override IEnumerable<string> GetAllMatchingPaths(string root, IEnumerable<string> patterns, string logFileName)
        {
            var allMatchingFiles = base.GetAllMatchingPaths(root, patterns, logFileName);

            return allMatchingFiles.Skip(1).Any() ?
                allMatchingFiles.OrderByDescending(path => File.GetLastWriteTime(path)).Take(1) :
                allMatchingFiles;
        }
    }
}
