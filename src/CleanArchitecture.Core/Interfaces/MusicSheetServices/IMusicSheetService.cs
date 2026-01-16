using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Interfaces.MusicSheetServices
{
    public interface IMusicSheetService
    {
        Task<ApiResult<DataTablePagedResult<MusicSheetResponse>>> ListByPagingAsync(MusicSheetPagingRequest request, CancellationToken cancellationToken);
        Task<ApiResult<MusicSheetResponse>> GetMusicSheetByIdAsync(int id);
        Task<ApiResult<int>> CreateMusicSheetAsync(MusicSheetRequest request, CancellationToken cancellationToken);
        Task<ApiResult<int>> UpdateMusicSheetAsync(int id, MusicSheetRequest request, CancellationToken cancellationToken);
        Task<ApiResult<int>> LikeAsync(int sheetId, CancellationToken cancellationToken);
        Task<ApiResult<int>> UnlikeAsync(int sheetId, CancellationToken cancellationToken);
        Task<FileResponse> ExportToImageAsync(string htmlContent);
    }
}
