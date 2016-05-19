namespace Lombiq.Vsix.Orchard.TemplateWizards.Services
{
    public enum LogType
    {
        Information,
        Warning,
        Error
    }


    /// <summary>
    /// Used for adding log entries somewhere based on the actual implementation.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Adds a log entry.
        /// </summary>
        /// <param name="logType">Type of the log entry (eg. information, error).</param>
        /// <param name="source">Source where the log entry was created (eg. name of the class).</param>
        /// <param name="text">Text of the log entry.</param>
        void Write(LogType logType, string source, string text);
    }


    public class NullLogger : ILogger
    {
        private static ILogger _instance;
        public static ILogger Instance
        {
            get
            {
                if (_instance == null) _instance = new NullLogger();

                return _instance;
            }
        }


        public void Write(LogType logType, string source, string text) { }
    }


    public static class LoggerExtensions
    {
        public static void Information(this ILogger logger, string source, string text)
        {
            logger.Write(LogType.Information, source, text);
        }

        public static void Warning(this ILogger logger, string source, string text)
        {
            logger.Write(LogType.Warning, source, text);
        }

        public static void Error(this ILogger logger, string source, string text)
        {
            logger.Write(LogType.Error, source, text);
        }
    }
}
