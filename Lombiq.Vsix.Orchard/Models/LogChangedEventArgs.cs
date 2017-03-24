using System;

namespace Lombiq.Vsix.Orchard.Models
{
    /// <summary>
    /// Contains details for the log file change event.
    /// </summary>
    public interface ILogChangedContext
    {
        /// <summary>
        /// Indicates whether the log file has content or it is empty.
        /// </summary>
        bool HasContent { get; }

        /// <summary>
        /// Full name of the log file.
        /// </summary>
        string FileName { get; }
    }


    public class LogChangedEventArgs : EventArgs, ILogChangedContext
    {
        public bool HasContent { get; set; }
        public string FileName { get; set; }
    }
}
