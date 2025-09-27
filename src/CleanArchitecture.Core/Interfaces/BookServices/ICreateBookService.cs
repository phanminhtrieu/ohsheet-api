using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.Core.Interfaces.BookServices
{
    public interface ICreateBookService
    {
        public Task<ApiResult<int>> CreateBookAsync(BookRequest request, CancellationToken cancellationToken);
    }
}
