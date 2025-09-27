using CleanArchitecture.Core.Domain.Entities.BookAggregate;
using CleanArchitecture.Core.Domain.Enums;
using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.Services.BookServices
{
    public class CreateBookService : ICreateBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateBookService> _logger;

        public CreateBookService(
            IBookRepository bookRepository, 
            IUnitOfWork unitOfWork,
            ILogger<CreateBookService> logger) 
        { 
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResult<int>> CreateBookAsync(BookRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Create Book with Title: {request.Title}, Author: {request.Author}, Status: {request.BookStatus}");

            var title = new BookTitle(request.Title);
            var author = new BookAuthor(request.Author);
            var bookStatus = (BookStatus)request.BookStatus;

            var book = Book.Create(title, author, bookStatus);

            await _bookRepository.AddAsync(book);
            
            return new ApiSuccessResult<int>(await _unitOfWork.SaveChangesAsync(cancellationToken));
        }
    }
}
