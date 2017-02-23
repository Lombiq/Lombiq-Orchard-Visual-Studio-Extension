using System.Text.RegularExpressions;

namespace Lombiq.Vsix.Orchard.Services
{
    public class FieldNameFromGenericTypeGenerator : FieldNameFromDependencyGeneratorBase
    {
        private const string GenericTypeNameRegexPattern = @"^[A-Z_]+[a-zA-Z0-9_]*[<]+[a-zA-Z_]+[a-zA-Z0-9_]*[>]+$";


        public override double Priority { get { return 10; } }


        public override bool CanGenerate(string dependency) { return Regex.IsMatch(dependency, GenericTypeNameRegexPattern); }

        public override string Generate(string dependency, bool useShortName)
        {
            var splittedDependency = dependency.Split('<');
            var genericType = splittedDependency[0];
            var genericParameter = splittedDependency[1].Substring(0, splittedDependency[1].Length - 1);

            var cleanedGenericType = RemoveFirstLetterIfInterface(genericType);
            var cleanedGenericParameter = RemoveFirstLetterIfInterface(genericParameter);

            return useShortName ? 
                    GetShortNameWithUnderscore(cleanedGenericParameter) + GetShortName(cleanedGenericType) : 
                    GetStringWithUnderscore(GetCamelCased(cleanedGenericParameter)) + cleanedGenericType;
        }
    }
}
