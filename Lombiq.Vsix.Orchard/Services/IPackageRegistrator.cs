using EnvDTE;
using System;
using System.ComponentModel.Design;

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
        /// <param name="dte"><see cref="DTE"/> instance required for acquiring general details about the environment.</param>
        /// <param name="menuCommandService"><see cref="IMenuCommandService"/> instance required for registering commands.</param>
        void RegisterCommands(DTE dte, IMenuCommandService menuCommandService);
    }
}
