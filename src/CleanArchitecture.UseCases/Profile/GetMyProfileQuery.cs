using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Core.Interfaces.UserServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Profile
{
    public record GetMyProfileQuery() : IRequest<ApiResult<UserProfileDto>>;

    public class GetMyProfileHandler(IProfileService _profileService) : IRequestHandler<GetMyProfileQuery, ApiResult<UserProfileDto>>
    {
        public async Task<ApiResult<UserProfileDto>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            return await _profileService.GetMyProfileAsync();
        }
    }
}
