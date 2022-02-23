using System;

namespace Lombiq.Vsix.Orchard.Models
{
    public class LogWatcherSettingsUpdatedEventArgs : EventArgs
    {
        public ILogWatcherSettings Settings { get; set; }
    }
}
