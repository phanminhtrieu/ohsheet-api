using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Interfaces.MusicSheetServices
{
    public interface IMusicSheetService
    {
        Task<ApiResult<MusicSheetResponse>> GetMusicSheetByIdAsync(int id);
        Task<ApiResult<int>> CreateMusicSheetAsync(MusicSheetRequest request, CancellationToken cancellationToken);
        Task<FileResponse> ExportToImageAsync(string htmlContent);
    }
}
