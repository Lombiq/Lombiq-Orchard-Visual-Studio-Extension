using System;

namespace Lombiq.Vsix.Orchard.Exceptions
{
    public static class DependencyToConstructorIjectorErrorCodes
    {
        public const string ConstructorNotFound = "ConstructorNotFound";
    }


    public class DependencyToConstructorInjectorException : Exception
    {
        public string ErrorCode { get; }


        public DependencyToConstructorInjectorException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
