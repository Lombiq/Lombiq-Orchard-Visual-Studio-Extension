using System;
using System.Runtime.Serialization;

namespace Lombiq.Vsix.Orchard.Exceptions
{
    public static class DependencyToConstructorIjectorErrorCodes
    {
        public const string ConstructorNotFound = "ConstructorNotFound";
    }

    [Serializable]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA1032:Implement standard exception constructors",
        Justification = "This exception needs the ErrorCode parameter in the constructor.")]
    public class DependencyToConstructorInjectorException : Exception
    {
        public string ErrorCode { get; set; }

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
