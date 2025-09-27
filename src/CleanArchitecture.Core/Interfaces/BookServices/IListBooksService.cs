using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Interfaces.BookServices
{
    public interface IListBooksService
    {
        public Task<ApiResult<List<BookResponse>>> ListBooksAsync(CancellationToken cancellationToken);
    }
}
