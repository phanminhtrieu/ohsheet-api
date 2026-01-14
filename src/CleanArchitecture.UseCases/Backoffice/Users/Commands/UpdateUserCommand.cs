using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using CleanArchitecture.Core.Interfaces;

namespace CleanArchitecture.UseCases.Backoffice.Users.Commands
{
    public record UpdateUserCommand(
        Guid UserId,
        string Email,
        string FirstName,
        string LastName,
        string Role,
        IFormFile? AvatarFile) : IRequest<ApiResult<bool>>;

    public class UpdateUserCommandHandler(
        UserManager<ApplicationUser> _userManager,
        RoleManager<ApplicationRole> _roleManager,
        IBlobService _blobService) : IRequestHandler<UpdateUserCommand, ApiResult<bool>>
    {
        public async Task<ApiResult<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User not found.");
            }

            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.UpdatedAt = DateTime.UtcNow;

            if (request.AvatarFile != null)
            {
                using var stream = request.AvatarFile.OpenReadStream();
                user.AvatarUrl = await _blobService.UploadAsync(stream, request.AvatarFile.FileName, request.AvatarFile.ContentType);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<bool>(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (!string.IsNullOrEmpty(request.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (!currentRoles.Contains(request.Role))
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    
                    if (!await _roleManager.RoleExistsAsync(request.Role))
                    {
                        await _roleManager.CreateAsync(new ApplicationRole { Name = request.Role });
                    }
                    await _userManager.AddToRoleAsync(user, request.Role);
                }
            }

            return new ApiSuccessResult<bool>(true);
        }
    }
}
