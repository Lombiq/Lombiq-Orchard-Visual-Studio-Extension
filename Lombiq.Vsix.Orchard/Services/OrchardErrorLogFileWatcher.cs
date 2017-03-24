using EnvDTE;
using System;
using System.IO;

namespace Lombiq.Vsix.Orchard.Services
{
    public class OrchardErrorLogFileWatcher : LogFileWatcherBase
    {
        private readonly ILogWatcherSettingsAccessor _logWatcherSettingsAccessor;
        private readonly DTE _dte;


        public OrchardErrorLogFileWatcher(ILogWatcherSettingsAccessor logWatcherSettingsAccessor, DTE dte)
        {
            _logWatcherSettingsAccessor = logWatcherSettingsAccessor;
            _dte = dte;
        }


        public override string GetLogFileName()
        {
            //var logFilePath = _logWatcherSettingsAccessor.GetSettings().LogFileFolderPath;
            //var solutionPath = _dte.Solution == null ? "" : Path.GetDirectoryName(_dte.Solution.FileName);
            //var errorLogFileName = "orchard-error-" + DateTime.Today.ToString("yyyy.MM.dd") + ".log";

            //return Path.Combine(solutionPath, logFilePath, errorLogFileName);

            return @"c:\Logtest\log.txt";
        }
    }
}
