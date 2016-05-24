using Microsoft.VisualStudio.Shell.Interop;

namespace Lombiq.Vsix.Orchard.TemplateWizards.Services
{
    public class VisualStudioActivityLogger : ILogger
    {
        private readonly IVsActivityLog _vsActivityLog;


        public VisualStudioActivityLogger(IVsActivityLog vsActivityLog)
        {
            _vsActivityLog = vsActivityLog;
        }


        public void Write(LogType logType, string source, string text)
        {
            uint vsLogType = 0;

            switch (logType)
            {
                case LogType.Information:
                    vsLogType = (uint)__ACTIVITYLOG_ENTRYTYPE.ALE_INFORMATION;
                    break;
                case LogType.Warning:
                    vsLogType = (uint)__ACTIVITYLOG_ENTRYTYPE.ALE_WARNING;
                    break;
                case LogType.Error:
                    vsLogType = (uint)__ACTIVITYLOG_ENTRYTYPE.ALE_ERROR;
                    break;
                default:
                    vsLogType = (uint)__ACTIVITYLOG_ENTRYTYPE.ALE_INFORMATION;
                    break;
            }

            _vsActivityLog.LogEntry(vsLogType, source, text);
        }
    }
}
