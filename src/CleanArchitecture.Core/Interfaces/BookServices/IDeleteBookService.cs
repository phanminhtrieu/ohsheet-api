using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Interfaces.BookServices
{
    public interface IDeleteBookService
    {
        public Task<ApiResult<int>> DeleteBookAsync(int bookId, CancellationToken cancellationToken);
    }
}
