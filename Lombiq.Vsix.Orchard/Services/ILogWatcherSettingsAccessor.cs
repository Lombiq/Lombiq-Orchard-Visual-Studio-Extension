using Lombiq.Vsix.Orchard.Models;

namespace Lombiq.Vsix.Orchard.Services
{
    public interface ILogWatcherSettingsAccessor
    {
        ILogWatcherSettings GetSettings();
    }
}
