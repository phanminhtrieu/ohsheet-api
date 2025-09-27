using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Books
{
    public record DeleteBookCommand(int bookId) : IRequest<ApiResult<int>> { }

    public class DeleteBookHandler(IDeleteBookService _bookService) : IRequestHandler<DeleteBookCommand, ApiResult<int>>
    {
        public async Task<ApiResult<int>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            return await _bookService.DeleteBookAsync(request.bookId, cancellationToken);
        }
    }
}
