using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Books
{
    public record ListBooksQuery() : IRequest<ApiResult<List<BookResponse>>> { }

    public class ListBooksHandler(IListBooksService _listBooksService) : IRequestHandler<ListBooksQuery, ApiResult<List<BookResponse>>>
    {
        public async Task<ApiResult<List<BookResponse>>> Handle(ListBooksQuery request, CancellationToken cancellationToken)
        {
            return await _listBooksService.ListBooksAsync(cancellationToken);
        }
    }
}
