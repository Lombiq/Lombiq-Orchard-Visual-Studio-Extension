using Lombiq.VisualStudioExtensions.Exceptions;
using System;
using System.IO;
using System.Linq;

namespace Lombiq.VisualStudioExtensions.Services
{
    public interface IDependencyToConstructorInjector
    {
        string Inject(string dependencyName, string code, string className);
    }
}
