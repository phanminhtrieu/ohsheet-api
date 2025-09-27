using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Interfaces.BookServices
{
    public interface IUpdateBookService
    {
        Task<ApiResult<int>> UpdateBookAsyn(int bookId, BookRequest request, CancellationToken cancellationToken);
    }
}
