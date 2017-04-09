namespace Lombiq.Vsix.Orchard.Models
{
    public interface IResult
    {
        bool Success { get; }

        string ErrorCode { get; }
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

        public static IResult FailedResult(string errorCode) => 
            new Result { Success = false, ErrorCode = errorCode };


        public bool Success { get; set; }

        public string ErrorCode { get; set; }
    }
}
