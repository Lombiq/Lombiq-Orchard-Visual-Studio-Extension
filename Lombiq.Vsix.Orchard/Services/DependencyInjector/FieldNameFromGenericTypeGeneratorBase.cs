using System.Text.RegularExpressions;

namespace Lombiq.Vsix.Orchard.Services.DependencyInjector
{
    public abstract class FieldNameFromGenericTypeGeneratorBase : FieldNameFromDependencyGeneratorBase
    {
        protected const string GenericTypeNameRegexPattern = @"^[A-Z_]+[a-zA-Z0-9_]*[<]+[a-zA-Z_]+[a-zA-Z0-9_]*[>]+$";


        public override bool CanGenerate(string dependency) =>
            Regex.IsMatch(dependency, GenericTypeNameRegexPattern);

        protected virtual CleanedGenericTypeSegments GetGenericTypeSegments(string dependency)
        {
            var splitDependency = dependency.Split('<');
            var genericType = splitDependency[0];
            var genericParameter = splitDependency[1].Substring(0, splitDependency[1].Length - 1);

            return new CleanedGenericTypeSegments
            {
                GenericTypeName = genericType,
                CleanedGenericTypeName = RemoveFirstLetterIfInterface(genericType),
                CleanedGenericParameterName = RemoveFirstLetterIfInterface(genericParameter)
            };
        }
        

        protected class CleanedGenericTypeSegments
        {
            public string GenericTypeName { get; set; }
            public string CleanedGenericTypeName { get; set; }
            public string CleanedGenericParameterName { get; set; }
        }
    }
}
