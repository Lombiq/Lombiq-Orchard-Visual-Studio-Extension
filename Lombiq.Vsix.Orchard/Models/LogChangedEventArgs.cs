using System;

namespace Lombiq.Vsix.Orchard.Models
{
    public interface ILogChangedContext
    {
        bool HasContent { get; }
        string Source { get; }
    }


    public class LogChangedEventArgs : EventArgs, ILogChangedContext
    {
        public bool HasContent { get; set; }
        public string Source { get; set; }
    }
}
