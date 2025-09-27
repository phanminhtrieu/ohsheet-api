using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Core.Interfaces.AuthServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Auth
{
    public record SignInCommand(UserSignInRequest UserSignInRequest) : IRequest<ApiResult<UserSignInResponse>> { }

    public class SignInHandler(IAuthService _authService) : IRequestHandler<SignInCommand, ApiResult<UserSignInResponse>>
    {
        public async Task<ApiResult<UserSignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            return await _authService.SignIn(request.UserSignInRequest);
        }
    }
}
