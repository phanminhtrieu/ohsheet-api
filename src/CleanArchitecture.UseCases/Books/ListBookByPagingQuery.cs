using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Books
{
    public record ListBookByPagingQuery(ManageBookPagingRequest Request) : IRequest<ApiResult<DataTablePagedResult<BookResponse>>> { }

    public class ListBookByPagingHandler(IListBooksByPagingService _listBooksByPagingService) 
        : IRequestHandler<ListBookByPagingQuery, ApiResult<DataTablePagedResult<BookResponse>>>
    {
        public async Task<ApiResult<DataTablePagedResult<BookResponse>>> Handle(ListBookByPagingQuery request, CancellationToken cancellationToken)
        {
            return await _listBooksByPagingService.ListBooksByPagingAsync(request.Request, cancellationToken);
        }
    }
}
