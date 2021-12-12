using System.Collections.Generic;

namespace Lombiq.Vsix.Orchard.Models
{
    public class DependencyName
    {
        public string Name { get; set; }
        public bool ShouldUseShortFieldNameByDefault { get; set; }
    }

    public class DependencyNameEqualityComparer : IEqualityComparer<DependencyName>
    {
        public bool Equals(DependencyName first, DependencyName second) =>
            first.Name == second.Name;

        public int GetHashCode(DependencyName dependencyName) =>
            dependencyName.Name.GetHashCode();
    }
}
