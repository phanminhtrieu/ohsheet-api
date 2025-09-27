using CleanArchitecture.Core.Domain.Entities.BookAggregate;
using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.Services.BookServices
{
    public class ListBooksService(IBookRepository _bookRepository, ILogger<Book> _logger) : IListBooksService
    {
        public async Task<ApiResult<List<BookResponse>>> ListBooksAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("List all books");

            var query = await _bookRepository.ListAsNoTrackingAsync(null, cancellationToken);
            var books = query.Select(b => new BookResponse(b.Id, b.Title.Value, b.Author.Value, (int)b.Status)).ToList();

            return new ApiSuccessResult<List<BookResponse>>(books);
        }
    }
}
