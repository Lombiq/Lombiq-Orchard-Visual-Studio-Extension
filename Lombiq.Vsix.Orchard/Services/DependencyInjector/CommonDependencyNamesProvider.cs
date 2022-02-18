using Lombiq.Vsix.Orchard.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lombiq.Vsix.Orchard.Services.DependencyInjector
{
    public class CommonDependencyNamesProvider : IDependencyNameProvider
    {
        private static readonly IEnumerable<string> CommonDependencyNames = new[]
        {
            "IAuthorizer",
            "IClock",
            "IContentDefinitionManager",
            "IContentItemDisplayManager",
            "IContentManager",
            "IDisplayManager<>",
            "IEnumerable<>",
            "IHtmlLocalizer<TClassName>",
            "IJsonConverter",
            "ILogger<TClassName>",
            "IMembershipService",
            "INotifier",
            "IOptions<>",
            "IOrchardServices",
            "IProjectionManager",
            "IRepository<>",
            "IScheduledTaskManager",
            "ISession",
            "IShapeDisplay",
            "IShapeFactory",
            "ISiteService",
            "IStringLocalizer<TClassName>",
            "ITokenizer",
            "ITransactionManager",
            "IUserEventHandler",
            "IUserService",
            "UrlHelper",
            "UserManager<>",
        };

        private static readonly IEnumerable<string> CommonDependencyNamesWhereShortNameShouldBeUsed = new[]
        {
            "IWorkContextAccessor",
            "IHttpContextAccessor",
        };

        public double Priority => 10;

        public IEnumerable<DependencyName> GetDependencyNames(string className = "") =>
            CommonDependencyNames
                .Select(dependencyName => CreateDependencyName(dependencyName, className))
                .Union(CommonDependencyNamesWhereShortNameShouldBeUsed
                    .Select(dependencyName => CreateDependencyName(dependencyName, className, shouldUseShortName: true)));

        private static DependencyName CreateDependencyName(string name, string className, bool shouldUseShortName = false) =>
            new DependencyName
            {
                Name = name.Replace("<TClassName>", $"<{className}>"),
                ShouldUseShortFieldNameByDefault = shouldUseShortName,
            };
    }
}
