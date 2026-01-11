using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicSheets
{
    public record CreateMusicSheetCommand(MusicSheetRequest Request) : IRequest<ApiResult<int>>
    {
    }

    public class CreateMusicSheetHandler(IMusicSheetService _musicSheetService) : IRequestHandler<CreateMusicSheetCommand, ApiResult<int>>
    {
        public async Task<ApiResult<int>> Handle(CreateMusicSheetCommand request, CancellationToken cancellationToken)
        {
            return await _musicSheetService.CreateMusicSheetAsync(request.Request, cancellationToken);
        }
    }
}
