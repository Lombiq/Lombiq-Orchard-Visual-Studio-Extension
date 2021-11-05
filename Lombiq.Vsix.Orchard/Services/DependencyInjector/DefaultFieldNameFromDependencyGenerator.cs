using Lombiq.Vsix.Orchard.Models;

namespace Lombiq.Vsix.Orchard.Services.DependencyInjector
{
    public class DefaultFieldNameFromDependencyGenerator : FieldNameFromDependencyGeneratorBase
    {
        public override double Priority => 1;

        public override bool CanGenerate(string dependency) => true;

        public override DependencyInjectionData Generate(string dependency, bool useShortName)
        {
            var dependencyInjectionData = new DependencyInjectionData
            {
                FieldType = dependency,
                ConstructorParameterType = dependency
            };

            // Default implementation with the lowest priority that generates the field and parameter name by adding
            // the underscore and shortens it if required.
            if (dependency.Length < 2)
            {
                dependencyInjectionData.FieldName = GetLowerInvariantStringWithUnderscore(dependency);
                dependencyInjectionData.ConstructorParameterName = GetLowerInvariantString(dependency);

                return dependencyInjectionData;
            }

            var cleanedDependency = RemoveFirstLetterIfInterface(dependency);

            if (useShortName)
            {
                dependencyInjectionData.FieldName = GetShortNameWithUnderscore(cleanedDependency);
                dependencyInjectionData.ConstructorParameterName = GetShortName(cleanedDependency);

                return dependencyInjectionData;
            }

            dependencyInjectionData.FieldName = GetStringWithUnderscore(GetCamelCased(cleanedDependency));
            dependencyInjectionData.ConstructorParameterName = GetCamelCased(cleanedDependency);

            return dependencyInjectionData;
        }
    }
}
