using System;

namespace Lombiq.Vsix.Orchard.Services
{
    public interface ILogWatcher
    {
        event EventHandler LogUpdated;

        void StartWatching();
        void StopWatching();
    }
}
