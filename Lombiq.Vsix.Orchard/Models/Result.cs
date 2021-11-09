namespace Lombiq.Vsix.Orchard.Models
{
    /// <summary>
    /// Interface for an operation result.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Gets a value indicating whether the result is successful.
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Gets the errors code.
        /// </summary>
        string ErrorCode { get; }
    }

    public class Result : IResult
    {
        private static IResult _successResult;

        public static IResult SuccessResult
        {
            get
            {
                _successResult = _successResult ?? new Result { Success = true };

                return _successResult;
            }
        }

        public static IResult FailedResult(string errorCode) =>
            new Result { Success = false, ErrorCode = errorCode };

        public bool Success { get; set; }

        public string ErrorCode { get; set; }
    }
}
