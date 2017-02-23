using System.Text.RegularExpressions;

namespace Lombiq.Vsix.Orchard.Services
{
    public class DefaultFieldNameFromGenericTypeGenerator : FieldNameFromGenericTypeGeneratorBase
    {
        private const string GenericTypeNameRegexPattern = @"^[A-Z_]+[a-zA-Z0-9_]*[<]+[a-zA-Z_]+[a-zA-Z0-9_]*[>]+$";


        public override double Priority { get { return 10; } }


        public override bool CanGenerate(string dependency) { return Regex.IsMatch(dependency, GenericTypeNameRegexPattern); }

        public override string Generate(string dependency, bool useShortName)
        {
            // Default implementation to handle dependencies with generic types. It places the generic parameter right
            // after the underscore and the generic type to the end.
            var segments = GetGenericTypeSegments(dependency);

            return useShortName ? 
                    GetShortNameWithUnderscore(segments.CleanedGenericParameterName) + GetShortName(segments.CleanedGenericTypeName) : 
                    GetStringWithUnderscore(GetCamelCased(segments.CleanedGenericParameterName)) + segments.CleanedGenericTypeName;
        }
    }
}
