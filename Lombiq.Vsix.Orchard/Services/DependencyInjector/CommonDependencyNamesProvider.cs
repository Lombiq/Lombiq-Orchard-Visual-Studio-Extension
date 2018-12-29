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
            "IHttpContextAccessor",
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
            "IWorkContextAccessor",
            "UrlHelper",
            "UserManager<>",
        };

        public double Priority => 10;


        public IEnumerable<string> GetDependencyNames(string className = "") =>
            CommonDependencyNames.Select(dependencyName => dependencyName.Replace("<TClassName>", $"<{className}>"));
    }
}
