using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Interfaces.UserServices
{
    public interface IProfileService
    {
        Task<ApiResult<UserProfileDto>> GetMyProfileAsync();
        Task<ApiResult<bool>> UpdateMyProfileAsync(UpdateUserProfileRequest request);
        Task<ApiResult<DataTablePagedResult<MusicSheetResponse>>> GetMyLikedSheetsAsync(PagingRequestBase request, CancellationToken cancellationToken);
        Task<ApiResult<DataTablePagedResult<MusicSheetResponse>>> GetMySheetsAsync(PagingRequestBase request, CancellationToken cancellationToken);
    }
}
