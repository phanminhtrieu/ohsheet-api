namespace CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[] ValidationErrors { get; set; }

        public ApiErrorResult()
        {
        }

        public ApiErrorResult(string message)
        {
            IsSucceeded = false;
            Message = message;
        }

        public ApiErrorResult(string[] validationErrors)
        {
            IsSucceeded = false;
            ValidationErrors = validationErrors;
        }
    }
}
