using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.Vsix.Orchard.Exceptions
{
    public static class DependencyToConstructorIjectorErrorCodes
    {
        public const string ConstructorNotFound = "ConstructorNotFound";
    }


    public class DependencyToConstructorInjectorException : Exception
    {
        public string ErrorCode { get; private set; }


        public DependencyToConstructorInjectorException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
