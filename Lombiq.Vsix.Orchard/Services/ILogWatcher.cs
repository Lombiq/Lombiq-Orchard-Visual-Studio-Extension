using Lombiq.Vsix.Orchard.Models;
using System;

namespace Lombiq.Vsix.Orchard.Services
{
    public interface ILogWatcher : IDisposable
    {
        event EventHandler<ILogChangedContext> LogUpdated;

        void StartWatching();
        void StopWatching();
        bool HasContent();
    }
}
