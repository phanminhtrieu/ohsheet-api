namespace CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T resultObj)
        {
            IsSucceeded = true;
            ResultObj = resultObj;
        }

        public ApiSuccessResult()
        {
            IsSucceeded = true;
        }
    }
}
