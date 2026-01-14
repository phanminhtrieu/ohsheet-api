using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.UseCases.Backoffice.Users.Commands
{
    public record LockUserCommand(Guid UserId, bool IsLocked) : IRequest<ApiResult<int>>;

    public class LockUserCommandHandler(UserManager<ApplicationUser> _userManager) : IRequestHandler<LockUserCommand, ApiResult<int>>
    {
        public async Task<ApiResult<int>> Handle(LockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<int>("User not found.");
            }

            if (request.IsLocked)
            {
                // Lock for 100 years
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            }
            else
            {
                // Unlock
                await _userManager.SetLockoutEndDateAsync(user, null);
            }

            return new ApiSuccessResult<int>(1);
        }
    }
}
