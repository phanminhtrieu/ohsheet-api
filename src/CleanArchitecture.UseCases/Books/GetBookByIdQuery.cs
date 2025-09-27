using CleanArchitecture.Core.Domain.Models.Books;
using CleanArchitecture.Core.Interfaces.BookServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Books
{
    public record GetBookByIdQuery(int Id) : IRequest<ApiResult<BookResponse>> { }

    public class GetBookByIdHandler(IGetBookByIdService _getBookService) : IRequestHandler<GetBookByIdQuery, ApiResult<BookResponse>>
    {
        public async Task<ApiResult<BookResponse>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            return await _getBookService.GetBookByIdAsync(request.Id, cancellationToken);
        }
    }
}
