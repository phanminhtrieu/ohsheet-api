using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicSheets
{
    public record UpdateMusicSheetCommand(int Id, MusicSheetRequest Request) : IRequest<ApiResult<int>>;

    public class UpdateMusicSheetCommandHandler(IMusicSheetService _musicSheetService) : IRequestHandler<UpdateMusicSheetCommand, ApiResult<int>>
    {
        public async Task<ApiResult<int>> Handle(UpdateMusicSheetCommand request, CancellationToken cancellationToken)
        {
            return await _musicSheetService.UpdateMusicSheetAsync(request.Id, request.Request, cancellationToken);
        }
    }
}
