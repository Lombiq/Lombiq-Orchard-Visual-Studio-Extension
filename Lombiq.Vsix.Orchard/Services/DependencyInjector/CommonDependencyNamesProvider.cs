using System.Collections.Generic;
using System.Linq;

namespace Lombiq.Vsix.Orchard.Services.DependencyInjector
{
    public class CommonDependencyNamesProvider : IDependencyNameProvider
    {
        private static readonly IEnumerable<string> CommonDependencyNames = new[]
        {
            "IWorkContextAccessor",
            "IHttpContextAccessor",
            "IOrchardServices",
            "IContentManager",
            "IRepository<>",
            "ILogger<T>",
            "IStringLocalizer<T>",
            "IHtmlLocalizer<T>"
        };

        public double Priority => 10;


        public IEnumerable<string> GetDependencyNames(string className = "") =>
            CommonDependencyNames.Select(dependencyName => dependencyName.Replace("<T>", $"<{className}>"));
    }
}
