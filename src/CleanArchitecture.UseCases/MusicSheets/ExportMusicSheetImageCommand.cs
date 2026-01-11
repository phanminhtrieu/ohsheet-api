using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Interfaces.MusicSheetServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.MusicSheets
{
    public record ExportMusicSheetImageCommand(ExportMusicSheetRequest Request) : IRequest<ApiResult<FileResponse>>
    {
    }

    public class ExportMusicSheetImageCommandHandler(IMusicSheetService _musicSheetService) : IRequestHandler<ExportMusicSheetImageCommand, ApiResult<FileResponse>>
    {
        public async Task<ApiResult<FileResponse>> Handle(ExportMusicSheetImageCommand request, CancellationToken cancellationToken)
        {
            var fileResponse = await _musicSheetService.ExportToImageAsync(request.Request.HtmlContent!);
            return new ApiSuccessResult<FileResponse>(fileResponse);
        }
    }
}
