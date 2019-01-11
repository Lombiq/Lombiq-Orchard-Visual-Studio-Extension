using System;

namespace Lombiq.Vsix.Orchard.Models
{
    public class DependencyInjectionData
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string ConstructorParameterName { get; set; }
        public string ConstructorParameterType { get; set; }
    }
}
