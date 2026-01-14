using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using CleanArchitecture.Core.Interfaces;

namespace CleanArchitecture.UseCases.Backoffice.Users.Commands
{
    public record CreateUserCommand(
        string UserName,
        string Email,
        string FirstName,
        string LastName,
        string Password,
        string Role,
        IFormFile? AvatarFile) : IRequest<ApiResult<Guid>>;

    public class CreateUserCommandHandler(
        UserManager<ApplicationUser> _userManager,
        RoleManager<ApplicationRole> _roleManager,
        IBlobService _blobService) : IRequestHandler<CreateUserCommand, ApiResult<Guid>>
    {
        public async Task<ApiResult<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByNameAsync(request.UserName);
            if (userExists != null)
            {
                return new ApiErrorResult<Guid>("Username already exists.");
            }

            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists != null)
            {
                return new ApiErrorResult<Guid>("Email already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (request.AvatarFile != null)
            {
                using var stream = request.AvatarFile.OpenReadStream();
                user.AvatarUrl = await _blobService.UploadAsync(stream, request.AvatarFile.FileName, request.AvatarFile.ContentType);
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<Guid>(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (!string.IsNullOrEmpty(request.Role))
            {
                if (!await _roleManager.RoleExistsAsync(request.Role))
                {
                    await _roleManager.CreateAsync(new ApplicationRole { Name = request.Role });
                }
                await _userManager.AddToRoleAsync(user, request.Role);
            }

            return new ApiSuccessResult<Guid>(user.Id);
        }
    }
}
