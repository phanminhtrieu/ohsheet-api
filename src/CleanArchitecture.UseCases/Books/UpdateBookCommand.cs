using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Books
{
    public record UpdateBookCommand(int BookId, BookRequest Request) : IRequest<ApiResult<int>> { }

    public class UpdateBookHandler(IUpdateBookService _updateBookService) : IRequestHandler<UpdateBookCommand, ApiResult<int>>
    {
        public async Task<ApiResult<int>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            return await _updateBookService.UpdateBookAsyn(request.BookId, request.Request, cancellationToken);
        }
    }
}
