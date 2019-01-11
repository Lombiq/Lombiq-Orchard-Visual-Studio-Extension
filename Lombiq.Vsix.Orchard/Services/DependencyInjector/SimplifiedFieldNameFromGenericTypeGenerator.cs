using Lombiq.Vsix.Orchard.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lombiq.Vsix.Orchard.Services.DependencyInjector
{
    public class SimplifiedFieldNameFromGenericTypeGenerator : FieldNameFromGenericTypeGeneratorBase
    {
        private static IEnumerable<string> SimplifiedGenericTypes = new[]
        {
            "UserManager",
            "ILogger"
        };


        public override double Priority => 15;


        public override bool CanGenerate(string dependency) => 
            base.CanGenerate(dependency) && SimplifiedGenericTypes.Any(type => dependency.StartsWith(type));

        public override DependencyInjectionData Generate(string dependency, bool useShortName)
        {
            // Default implementation to handle dependencies with generic types. It places the generic parameter right
            // after the underscore and the generic type to the end.
            var segments = GetGenericTypeSegments(dependency);

            return new DependencyInjectionData
            {
                FieldName = useShortName ?
                    GetShortNameWithUnderscore(segments.CleanedGenericTypeName) :
                    GetStringWithUnderscore(GetCamelCased(segments.CleanedGenericTypeName)),
                FieldType = dependency,
                ConstructorParameterName = useShortName ?
                    GetShortName(GetCamelCased(segments.CleanedGenericTypeName)) :
                    GetCamelCased(segments.CleanedGenericTypeName),
                ConstructorParameterType = dependency
            };
        }
    }
}
