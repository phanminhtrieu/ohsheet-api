using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicSheets
{
    public record UnlikeMusicSheetCommand(int SheetId) : IRequest<ApiResult<int>>
    {
    }

    public class UnlikeMusicSheetHandler(IMusicSheetService _musicSheetService) : IRequestHandler<UnlikeMusicSheetCommand, ApiResult<int>>
    {
        public async Task<ApiResult<int>> Handle(UnlikeMusicSheetCommand request, CancellationToken cancellationToken)
        {
            return await _musicSheetService.UnlikeAsync(request.SheetId, cancellationToken);
        }
    }
}
