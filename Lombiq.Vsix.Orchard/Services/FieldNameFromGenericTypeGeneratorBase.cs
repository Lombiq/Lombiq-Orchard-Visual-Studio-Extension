namespace Lombiq.Vsix.Orchard.Services
{
    public abstract class FieldNameFromGenericTypeGeneratorBase : FieldNameFromDependencyGeneratorBase
    {
        protected virtual CleanedGenericTypeSegments GetGenericTypeSegments(string dependency)
        {
            var splitDependency = dependency.Split('<');
            var genericType = splitDependency[0];
            var genericParameter = splitDependency[1].Substring(0, splitDependency[1].Length - 1);

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
