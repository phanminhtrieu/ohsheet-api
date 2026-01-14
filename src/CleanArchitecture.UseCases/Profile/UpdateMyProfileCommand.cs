using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Core.Interfaces.UserServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Profile
{
    public record UpdateMyProfileCommand(UpdateUserProfileRequest Request) : IRequest<ApiResult<bool>>;

    public class UpdateMyProfileHandler(IProfileService _profileService) : IRequestHandler<UpdateMyProfileCommand, ApiResult<bool>>
    {
        public async Task<ApiResult<bool>> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
        {
            return await _profileService.UpdateMyProfileAsync(request.Request);
        }
    }
}
