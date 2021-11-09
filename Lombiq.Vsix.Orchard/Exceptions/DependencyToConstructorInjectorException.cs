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
        public string ErrorCode { get; set; }

        protected DependencyToConstructorInjectorException()
        {
        }

        protected DependencyToConstructorInjectorException(string message)
            : base(message)
        {
        }

        protected DependencyToConstructorInjectorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DependencyToConstructorInjectorException(string errorCode, string message)
            : base(message) => ErrorCode = errorCode;

        protected DependencyToConstructorInjectorException(SerializationInfo info, StreamingContext context)
            : base(info, context) => ErrorCode = (string)info.GetValue(nameof(ErrorCode), typeof(string));

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(ErrorCode), ErrorCode);
        }
    }
}
