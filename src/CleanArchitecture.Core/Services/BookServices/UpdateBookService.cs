using CleanArchitecture.Core.Domain.Entities.BookAggregate;
using CleanArchitecture.Core.Domain.Entities.BookAggregate.Specifications;
using CleanArchitecture.Core.Domain.Enums;
using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using CleanArchitecture.Shared.Common.Exceptions;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.Services.BookServices
{
    public class UpdateBookService(
        IBookRepository _bookRepository,
        IUnitOfWork _unitOfWork,
        ILogger<Book> _logger) : IUpdateBookService
    {
        public async Task<ApiResult<int>> UpdateBookAsyn(int bookId, BookRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Updating Book with Id: {bookId}, Title: {request.Title}, Author: {request.Author}, Status: {request.BookStatus}");

            var bookByIdSpec = new BookByIdSpec(bookId);
            var book = await _bookRepository.GetAsync(bookByIdSpec, cancellationToken);

            var isBookExisted = book != null;

            if (!isBookExisted)
            {
                _logger.LogWarning($"Book {bookId} not found");

                throw UserException.InternalServerException();
            }

            book!.Update(new BookTitle(request.Title), new BookAuthor(request.Author), (BookStatus)request.BookStatus);

            _bookRepository.Update(book!);

            return new ApiSuccessResult<int>(await _unitOfWork.SaveChangesAsync(cancellationToken));
        }
    }
}
