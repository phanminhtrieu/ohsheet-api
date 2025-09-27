using CleanArchitecture.Core.Domain.Entities.BookAggregate;
using CleanArchitecture.Core.Domain.Entities.BookAggregate.Specifications;
using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Shared.Common.Exceptions;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.Services.BookServices
{
    public class GetBookByIdService(IBookRepository _bookRepository, ILogger<Book> _looger) : IGetBookByIdService
    {
        public async Task<ApiResult<BookResponse>> GetBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            _looger.LogInformation($"Get book by Id: {id}");

            var spec = new BookByIdSpec(id);
            var book = await _bookRepository.GetAsync(spec, cancellationToken, null);

            var isBookExisted = book != null;

            if (!isBookExisted)
            {
                _looger.LogWarning($"Cannot get book by Id: {id}");

                throw UserException.InternalServerException();
            }

            var response = new BookResponse(book.Id, book.Title.Value, book.Author.Value, (int)book.Status);

            return new ApiSuccessResult<BookResponse>(response);
        }
    }
}   
