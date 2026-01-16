using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Repositories;
using MediatR;

namespace CleanArchitecture.UseCases.Backoffice.MusicSheets.Queries
{
    public record ListAllMusicSheetsQuery(MusicSheetPagingRequest Request) : IRequest<ApiResult<DataTablePagedResult<MusicSheetResponse>>>;

    public class ListAllMusicSheetsQueryHandler(IMusicSheetRepository _musicSheetRepository) : IRequestHandler<ListAllMusicSheetsQuery, ApiResult<DataTablePagedResult<MusicSheetResponse>>>
    {
        public async Task<ApiResult<DataTablePagedResult<MusicSheetResponse>>> Handle(ListAllMusicSheetsQuery request, CancellationToken cancellationToken)
        {
            var result = await _musicSheetRepository.ListByPagingAsync(request.Request, null, cancellationToken);
            return new ApiSuccessResult<DataTablePagedResult<MusicSheetResponse>>(result);
        }
    }
}
