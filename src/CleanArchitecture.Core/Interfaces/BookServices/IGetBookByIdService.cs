using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Interfaces.BookServices
{
    public interface IGetBookByIdService
    {
        Task<ApiResult<BookResponse>> GetBookByIdAsync(int id, CancellationToken cancellationToken);
    }
}
