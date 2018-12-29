using Lombiq.Vsix.Orchard.Models;
using System.Text.RegularExpressions;

namespace Lombiq.Vsix.Orchard.Services.DependencyInjector
{
    public class DefaultFieldNameFromGenericTypeGenerator : FieldNameFromGenericTypeGeneratorBase
    {
        private const string GenericTypeNameRegexPattern = @"^[A-Z_]+[a-zA-Z0-9_]*[<]+[a-zA-Z_]+[a-zA-Z0-9_]*[>]+$";


        public override double Priority => 10;


        public override bool CanGenerate(string dependency) => 
            Regex.IsMatch(dependency, GenericTypeNameRegexPattern);

        public override DependencyInjectionData Generate(string dependency, bool useShortName)
        {
            // Default implementation to handle dependencies with generic types. It places the generic parameter right
            // after the underscore and the generic type to the end.
            var segments = GetGenericTypeSegments(dependency);

            return new DependencyInjectionData
            {
                FieldName = useShortName ?
                    GetShortNameWithUnderscore(segments.CleanedGenericParameterName) + GetShortName(segments.CleanedGenericTypeName) :
                    GetStringWithUnderscore(GetCamelCased(segments.CleanedGenericParameterName)) + segments.CleanedGenericTypeName,
                FieldType = dependency,
                ConstructorParameterName = useShortName ?
                    GetShortName(segments.CleanedGenericParameterName) + GetShortName(segments.CleanedGenericTypeName) :
                    GetCamelCased(segments.CleanedGenericParameterName) + segments.CleanedGenericTypeName,
                ConstructorParameterType = dependency
            };
        }
    }
}
