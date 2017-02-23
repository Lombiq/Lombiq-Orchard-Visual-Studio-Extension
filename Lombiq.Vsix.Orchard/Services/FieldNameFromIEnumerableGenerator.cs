using System.Text.RegularExpressions;

namespace Lombiq.Vsix.Orchard.Services
{
    public class FieldNameFromIEnumerableGenerator : FieldNameFromGenericTypeGeneratorBase
    {
        private const string IEnumerableNameRegexPattern = @"^IEnumerable[<]+[a-zA-Z_]+[a-zA-Z0-9_]*[>]+$";


        public override double Priority { get { return 15; } }


        public override bool CanGenerate(string dependency) { return Regex.IsMatch(dependency, IEnumerableNameRegexPattern); }

        public override string Generate(string dependency, bool useShortName)
        {
            var segments = GetGenericTypeSegments(dependency);
            
            return (useShortName ? 
                GetShortNameWithUnderscore(segments.CleanedGenericParameterName) : 
                GetStringWithUnderscore(GetCamelCased(segments.CleanedGenericParameterName))) 
                + "s";
        }
    }
}
