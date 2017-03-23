using EnvDTE;
using System;
using System.IO;

namespace Lombiq.Vsix.Orchard.Services.LogWatcher
{
    public class ErrorLogFileWatcher : LogFileWatcherBase
    {
        private readonly ILogWatcherSettingsAccessor _logWatcherSettingsAccessor;
        private readonly DTE _dte;


        public ErrorLogFileWatcher(ILogWatcherSettingsAccessor logWatcherSettingsAccessor, DTE dte)
        {
            _logWatcherSettingsAccessor = logWatcherSettingsAccessor;
            _dte = dte;
        }


        protected override string GetLogFileName()
        {
            var logFilePath = _logWatcherSettingsAccessor.GetSettings().LogFileFolderPath;
            var solutionPath = _dte.Solution == null ? "" : Path.GetDirectoryName(_dte.Solution.FileName);
            var errorLogFileName = "orchard-error-" + DateTime.Today.ToString("yyyy.MM.dd") + ".log";

            return Path.Combine(solutionPath, logFilePath, errorLogFileName);
        }
    }
}
