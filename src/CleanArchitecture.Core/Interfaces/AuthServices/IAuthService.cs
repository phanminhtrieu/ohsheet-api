using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;

namespace CleanArchitecture.Core.Interfaces.AuthServices
{
    public interface IAuthService
    {
        Task<ApiResult<UserSignInResponse>> SignIn(UserSignInRequest request);
        Task<ApiResult<UserSignUpResponse>> SignUp(UserSignUpRequest request, CancellationToken cancellationToken);
        bool Logout();
        Task<ApiResult<string>> RefreshToken();
        Task<ApiResult<UserProfileResponse>> GetProfile(CancellationToken cancellationToken);
    }
}
