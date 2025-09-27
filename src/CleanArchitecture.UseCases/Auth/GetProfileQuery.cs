using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Core.Interfaces.AuthServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Auth
{
    public record GetProfileQuery() : IRequest<ApiResult<UserProfileResponse>> { }

    public class GetProfileHandler(IAuthService _authService) : IRequestHandler<GetProfileQuery, ApiResult<UserProfileResponse>>
    {
        public Task<ApiResult<UserProfileResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            return _authService.GetProfile(cancellationToken);
        }
    }
}
