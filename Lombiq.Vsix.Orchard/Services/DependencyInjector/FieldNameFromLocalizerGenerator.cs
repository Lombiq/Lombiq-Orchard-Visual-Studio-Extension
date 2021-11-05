using Lombiq.Vsix.Orchard.Models;
using System.Text.RegularExpressions;

namespace Lombiq.Vsix.Orchard.Services.DependencyInjector
{
    public class FieldNameFromLocalizerGenerator : FieldNameFromGenericTypeGeneratorBase
    {
        private const string Pattern = @"^(IStringLocalizer|IHtmlLocalizer)[<]+[a-zA-Z_]+[a-zA-Z0-9_]*[>]+$";

        public override double Priority => 15;

        public override bool CanGenerate(string dependency) =>
            Regex.IsMatch(dependency, Pattern);

        public override DependencyInjectionData Generate(string dependency, bool useShortName)
        {
            // This implementation handles only the dependencies with IEnumerable<T> generic types. It places the
            // generic parameter right after the underscore using its plural form.
            var segments = GetGenericTypeSegments(dependency);

            return new DependencyInjectionData
            {
                FieldName = dependency.StartsWith("IHtmlLocalizer") ? "H" : "T",
                FieldType = segments.GenericTypeName,
                ConstructorParameterName = useShortName ?
                    GetCamelCased(GetShortName(segments.CleanedGenericTypeName)) :
                    GetCamelCased(segments.CleanedGenericTypeName),
                ConstructorParameterType = dependency,
            };
        }
    }
}
