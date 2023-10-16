using System;
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
        public bool Equals(DependencyName x, DependencyName y) => x.Name == y.Name;

        public int GetHashCode(DependencyName obj) => StringComparer.Ordinal.GetHashCode(obj);
    }
}
