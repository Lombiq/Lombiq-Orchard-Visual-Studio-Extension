using System;

namespace Lombiq.Vsix.Orchard.Models
{
    public class LogChangedEventArgs : EventArgs
    {
        public ILogFileStatus LogFileStatus { get; set; }
    }
}
