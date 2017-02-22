using System.Linq;

namespace Lombiq.Vsix.Orchard.Services
{
    public class DefaultFieldNameFromDependencyGenerator : IFieldNameFromDependencyGenerator
    {
        public double Priority { get { return 1; } }


        public bool CanGenerate(string dependency) { return true; }

        public string Generate(string dependency, bool useShortName)
        {
            if (dependency.Length < 2) return "_" + dependency.ToLowerInvariant();

            var cleanedDependency = dependency.Length > 1 && dependency.StartsWith("I") && char.IsUpper(dependency[1]) ? 
                dependency.Substring(1) : 
                string.Copy(dependency);

            if (dependency.Length < 2) return "_" + dependency.ToLowerInvariant();

            if (useShortName)
            {
                var upperCasedLetters = cleanedDependency.Where(letter => char.IsUpper(letter));

                return upperCasedLetters.Any() ? 
                    ("_" + new string(upperCasedLetters.ToArray())).ToLowerInvariant() : 
                    "_" + cleanedDependency[0];
            }

            return "_" + char.ToLower(cleanedDependency[0]) + cleanedDependency.Substring(1);
        }
    }
}
