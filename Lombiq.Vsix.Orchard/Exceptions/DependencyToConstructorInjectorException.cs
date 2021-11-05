using System;
using System.Runtime.Serialization;

namespace Lombiq.Vsix.Orchard.Exceptions
{
    public static class DependencyToConstructorIjectorErrorCodes
    {
        public const string ConstructorNotFound = "ConstructorNotFound";
    }

    [Serializable]
    public class DependencyToConstructorInjectorException : Exception
    {
        public string ErrorCode { get; }

        public DependencyToConstructorInjectorException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(ErrorCode), ErrorCode);
        }
    }
}
