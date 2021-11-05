using System;

namespace Lombiq.Vsix.Orchard.Models
{
    /// <summary>
    /// Contains details for the log file change event.
    /// </summary>
    public interface ILogFileStatus
    {
        /// <summary>
        /// Gets a value indicating whether the log file has content if it exists or it is empty.
        /// </summary>
        bool HasContent { get; }

        /// <summary>
        /// Gets a value indicating whether the log file exists or not.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Gets the full path of the log file.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the last time when the log file has been updated in UTC.
        /// </summary>
        DateTime? LastUpdatedUtc { get; }
    }

    public class LogFileStatus : ILogFileStatus
    {
        public bool HasContent { get; set; }
        public bool Exists { get; set; }
        public string Path { get; set; }
        public DateTime? LastUpdatedUtc { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ILogFileStatus logFileStatus)) return false;

            return HasContent == logFileStatus.HasContent &&
                Exists == logFileStatus.Exists &&
                Path == logFileStatus.Path &&
                LastUpdatedUtc == logFileStatus.LastUpdatedUtc;
        }

        public override int GetHashCode() =>
            base.GetHashCode();
    }
}
