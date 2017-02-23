using System.Text.RegularExpressions;

namespace Lombiq.Vsix.Orchard.Services
{
    public class FieldNameFromIEnumerableGenerator : FieldNameFromDependencyGeneratorBase
    {
        private const string IEnumerableNameRegexPattern = @"^IEnumerable[<]+[a-zA-Z_]+[a-zA-Z0-9_]*[>]+$";


        public override double Priority { get { return 15; } }


        public override bool CanGenerate(string dependency) { return Regex.IsMatch(dependency, IEnumerableNameRegexPattern); }

        public override string Generate(string dependency, bool useShortName)
        {
            var splittedDependency = dependency.Split('<');
            var iEnumerableType = splittedDependency[0];
            var genericParameter = splittedDependency[1].Substring(0, splittedDependency[1].Length - 1);

            var cleanedIEnumerableType = RemoveFirstLetterIfInterface(iEnumerableType);
            var cleanedGenericParameter = RemoveFirstLetterIfInterface(genericParameter);

            return (useShortName ? 
                GetShortNameWithUnderscore(cleanedGenericParameter) : 
                GetStringWithUnderscore(GetCamelCased(cleanedGenericParameter))) 
                + "s";
        }
    }
}
