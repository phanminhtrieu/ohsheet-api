using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.UserServices;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using CleanArchitecture.Shared.Common.Errors;
using CleanArchitecture.Shared.Common.Exceptions;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Core.Services.UserServices
{
    public class ProfileService(
        UserManager<ApplicationUser> _userManager,
        IMusicSheetRepository _musicSheetRepository,
        ICurrentUserService _currentUserService,
        IUnitOfWork _unitOfWork,
        IBlobService _blobService) : IProfileService
    {
        public async Task<ApiResult<UserProfileDto>> GetMyProfileAsync()
        {
            var userId = _currentUserService.UserGuid;
            if (userId == null) throw new UserFriendlyException(ErrorCode.Unauthorized, "User not authenticated", "User not authenticated");

            var user = await _userManager.FindByIdAsync(userId.ToString()!);
            if (user == null) throw new UserFriendlyException(ErrorCode.NotFound, "User not found", "User not found");

            var profile = new UserProfileDto
            {
                UserId = user.Id,
                FullName = user.DisplayName ?? $"{user.FirstName} {user.LastName}".Trim(),
                Email = user.Email!,
                AvatarUrl = user.AvatarUrl,
                Bio = user.Bio,
                JoinedDate = user.CreatedAt
            };

            return new ApiSuccessResult<UserProfileDto>(profile);
        }

        public async Task<ApiResult<bool>> UpdateMyProfileAsync(UpdateUserProfileRequest request)
        {
            var userId = _currentUserService.UserGuid;
            if (userId == null) throw new UserFriendlyException(ErrorCode.Unauthorized, "User not authenticated", "User not authenticated");

            var user = await _userManager.FindByIdAsync(userId.ToString()!);
            if (user == null) throw new UserFriendlyException(ErrorCode.NotFound, "User not found", "User not found");

            if (request.AvatarFile != null)
            {
                using var stream = request.AvatarFile.OpenReadStream();
                user.AvatarUrl = await _blobService.UploadAsync(stream, request.AvatarFile.FileName, request.AvatarFile.ContentType);
            }
            else if (!string.IsNullOrEmpty(request.AvatarUrl))
            {
                user.AvatarUrl = request.AvatarUrl;
            }

            user.DisplayName = request.FullName;
            user.Bio = request.Bio;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ApiErrorResult<bool>(errors);
            }

            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<DataTablePagedResult<MusicSheetResponse>>> GetMyLikedSheetsAsync(PagingRequestBase request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserGuid;
            if (userId == null) throw new UserFriendlyException(ErrorCode.Unauthorized, "User not authenticated", "User not authenticated");

            var pagedResult = await _musicSheetRepository.ListLikedByPagingAsync(request, userId.Value, cancellationToken);

            return new ApiSuccessResult<DataTablePagedResult<MusicSheetResponse>>(pagedResult);
        }

        public async Task<ApiResult<DataTablePagedResult<MusicSheetResponse>>> GetMySheetsAsync(PagingRequestBase request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserGuid;
            if (userId == null) throw new UserFriendlyException(ErrorCode.Unauthorized, "User not authenticated", "User not authenticated");

            var pagedResult = await _musicSheetRepository.ListByAuthorPagingAsync(request, userId.Value, cancellationToken);

            return new ApiSuccessResult<DataTablePagedResult<MusicSheetResponse>>(pagedResult);
        }
    }
}
