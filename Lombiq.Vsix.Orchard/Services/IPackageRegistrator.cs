using System;

namespace Lombiq.Vsix.Orchard.Services
{
    /// <summary>
    /// Registers components required for the package (e.g. registers commands, event listeners etc.).
    /// </summary>
    public interface IPackageRegistrator : IDisposable
    {
        /// <summary>
        /// Registers commands and related components. Also registers the related event handlers.
        /// </summary>
        void RegisterCommands();
    }
}
