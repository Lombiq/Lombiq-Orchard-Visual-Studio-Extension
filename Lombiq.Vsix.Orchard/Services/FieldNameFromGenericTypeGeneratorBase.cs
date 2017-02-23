using System.Text.RegularExpressions;

namespace Lombiq.Vsix.Orchard.Services
{
    public abstract class FieldNameFromGenericTypeGeneratorBase : FieldNameFromDependencyGeneratorBase
    {
        protected virtual CleanedGenericTypeSegments GetGenericTypeSegments(string dependency)
        {
            var splittedDependency = dependency.Split('<');
            var genericType = splittedDependency[0];
            var genericParameter = splittedDependency[1].Substring(0, splittedDependency[1].Length - 1);

            return new CleanedGenericTypeSegments
            {
                CleanedGenericTypeName = RemoveFirstLetterIfInterface(genericType),
                CleanedGenericParameterName = RemoveFirstLetterIfInterface(genericParameter)
            };
        }



        protected class CleanedGenericTypeSegments
        {
            public string CleanedGenericTypeName { get; set; }
            public string CleanedGenericParameterName { get; set; }
        }
    }
}
