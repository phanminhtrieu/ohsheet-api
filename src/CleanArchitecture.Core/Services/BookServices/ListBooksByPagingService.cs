using CleanArchitecture.Core.Domain.Entities.BookAggregate;
using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Shared.CrossCuttingConcerns.Filtering;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CleanArchitecture.Core.Services.BookServices
{
    public class ListBooksByPagingService(IBookRepository _bookRepository, ILogger<Book> _logger) : IListBooksByPagingService
    {
        public async Task<ApiResult<DataTablePagedResult<BookResponse>>> ListBooksByPagingAsync(ManageBookPagingRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"List Books By Paging with TextSearch: {request.TextSearch}, PageIndex: {request.PageIndex}, PageSize: {request.PageSize}");

            Expression<Func<Book, bool>> filter = x => true;

            var isTextSearchExisted = !string.IsNullOrEmpty(request.TextSearch);

            if (isTextSearchExisted)
            {
                filter = ExpressionExtension.CombineExpressions(filter, book => book.Title.Value.Contains(request.TextSearch!) );
            }

            var pageResult = await _bookRepository.ListByPagingAsync(request, filter, book => MapToDto(book),cancellationToken, null);

            return new ApiSuccessResult<DataTablePagedResult<BookResponse>>(pageResult);
        }

        private static BookResponse MapToDto(Book book)
        {
            return new BookResponse(book.Id, book.Title.Value, book.Author.Value, (int)book.Status);
        }
    }
}
