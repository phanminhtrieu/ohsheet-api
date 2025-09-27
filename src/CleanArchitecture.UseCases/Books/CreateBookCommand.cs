using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Books
{
    public record CreateBookCommand(BookRequest Request) : IRequest<ApiResult<int>> { }

    public class CreateBookHandler : IRequestHandler<CreateBookCommand, ApiResult<int>>
    {
        private readonly ICreateBookService _createBookService;

        public CreateBookHandler(ICreateBookService createBookService)
        {
            _createBookService = createBookService;
        }

        public async Task<ApiResult<int>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            return await _createBookService.CreateBookAsync(request.Request, cancellationToken);
        }
    }
}
