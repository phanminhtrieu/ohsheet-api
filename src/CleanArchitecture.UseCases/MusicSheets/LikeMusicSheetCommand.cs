using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicSheets
{
    public record LikeMusicSheetCommand(int SheetId) : IRequest<ApiResult<int>>
    {
    }

    public class LikeMusicSheetHandler(IMusicSheetService _musicSheetService) : IRequestHandler<LikeMusicSheetCommand, ApiResult<int>>
    {
        public async Task<ApiResult<int>> Handle(LikeMusicSheetCommand request, CancellationToken cancellationToken)
        {
            return await _musicSheetService.LikeAsync(request.SheetId, cancellationToken);
        }
    }
}
