using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Interfaces.BookServices
{
    public interface IListBooksByPagingService
    {
        public Task<ApiResult<DataTablePagedResult<BookResponse>>> ListBooksByPagingAsync(ManageBookPagingRequest request, CancellationToken cancellation);
    }
}
