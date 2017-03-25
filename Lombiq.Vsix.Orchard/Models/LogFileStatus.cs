using System;
using System.Collections.Generic;

namespace Lombiq.Vsix.Orchard.Models
{
    /// <summary>
    /// Contains details for the log file change event.
    /// </summary>
    public interface ILogFileStatus
    {
        /// <summary>
        /// Indicates whether the log file has content if it exists or it is empty.
        /// </summary>
        bool HasContent { get; }

        /// <summary>
        /// Indicates whether the log file exists or not.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Full name of the log file.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Last time when the log file has been updated in UTC.
        /// </summary>
        DateTime? LastUpdatedUtc { get; }
    }

    public class LogFileStatus : ILogFileStatus
    {
        public bool HasContent { get; set; }
        public bool Exists { get; set; }
        public string FileName { get; set; }
        public DateTime? LastUpdatedUtc { get; set; }


        public override bool Equals(object logFileStatusObject)
        {
            var logFileStatus = logFileStatusObject as ILogFileStatus;

            if (logFileStatus == null) return false;

            return HasContent == logFileStatus.HasContent && 
                Exists == logFileStatus.Exists &&
                FileName == logFileStatus.FileName &&
                LastUpdatedUtc == logFileStatus.LastUpdatedUtc;
        }

        public override int GetHashCode() =>
            base.GetHashCode();
    }
}
