using Lombiq.Vsix.Orchard.Models;
using System.Collections.Generic;

namespace Lombiq.Vsix.Orchard.Services.DependencyInjector
{
    /// <summary>
    /// Service for providing dependency names. Can be used as an auto-complete source for dependency name editors.
    /// </summary>
    public interface IDependencyNameProvider
    {
        /// <summary>
        /// Priority number used to decide which provider implementation needs to provide dependency names first.
        /// </summary>
        double Priority { get; }


        /// <summary>
        /// Returns dependency names and some suggested information about them (e.g. should use short field name).
        /// </summary>
        /// <param name="className">Name of the class the dependency is injected to. Can be used to provide
        /// class-specific dependency names like IStringLocalizer<TClass>.</param>
        /// <returns>Dependency names and suggested information about them.</returns>
        IEnumerable<DependencyName> GetDependencyNames(string className = "");
    }
}
