namespace Lombiq.VisualStudioExtensions.Models
{
    public interface IResult
    {
        bool Success { get; }

        string ErrorMessage { get; }
    }


    public class Result : IResult
    {
        private static IResult _successResult;

        public static IResult SuccessResult 
        {
            get
            {
                if (_successResult == null) _successResult = new Result { Success = true };

                return _successResult;
            }
        }

        public static IResult FailedResult(string message)
        {
            return new Result { Success = false, ErrorMessage = message };
        }


        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
