using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicSheets
{
    public record ListMusicSheetByPagingQuery(MusicSheetPagingRequest Request) : IRequest<ApiResult<DataTablePagedResult<MusicSheetResponse>>>
    {
    }

    public class ListMusicSheetByPagingHandler(IMusicSheetService _musicSheetService) : IRequestHandler<ListMusicSheetByPagingQuery, ApiResult<DataTablePagedResult<MusicSheetResponse>>>
    {
        public async Task<ApiResult<DataTablePagedResult<MusicSheetResponse>>> Handle(ListMusicSheetByPagingQuery query, CancellationToken cancellationToken)
        {
            return await _musicSheetService.ListByPagingAsync(query.Request, cancellationToken);
        }
    }
}
