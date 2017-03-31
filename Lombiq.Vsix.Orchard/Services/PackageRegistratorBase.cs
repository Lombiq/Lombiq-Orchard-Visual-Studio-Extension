using EnvDTE;
using System.ComponentModel.Design;

namespace Lombiq.Vsix.Orchard.Services
{
    public abstract class PackageRegistratorBase : IPackageRegistrator
    {
        protected readonly IMenuCommandService _menuCommandService;
        protected readonly DTE _dte;


        public PackageRegistratorBase(DTE dte, IMenuCommandService menuCommandService)
        {
            _dte = dte;
            _menuCommandService = menuCommandService;
        }


        public virtual void Dispose() { }

        public virtual void RegisterCommands() { }
    }
}
