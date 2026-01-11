using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicSheets
{
    public record GetMusicSheetByIdQuery(int Id) : IRequest<ApiResult<MusicSheetResponse>>
    {
    }

    public class GetMusicSheetByIdHandler(
        IMusicSheetService _musicSheetService,
        IMusicSheetViewService _musicSheetViewService) : IRequestHandler<GetMusicSheetByIdQuery, ApiResult<MusicSheetResponse>>
    {
        public async Task<ApiResult<MusicSheetResponse>> Handle(GetMusicSheetByIdQuery request, CancellationToken cancellationToken)
        {
            var musicSheet = await _musicSheetService.GetMusicSheetByIdAsync(request.Id);

            await _musicSheetViewService.IncrementViewCount(request.Id);

            return musicSheet;
        }
    }
}
