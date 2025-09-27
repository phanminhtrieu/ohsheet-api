using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Core.Interfaces.AuthServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Auth
{
    public record SignUpCommand(UserSignUpRequest UserSignUpRequest) : IRequest<ApiResult<UserSignUpResponse>> { }

    public class SignUpHandler(IAuthService _authService) : IRequestHandler<SignUpCommand, ApiResult<UserSignUpResponse>>
    {
        public Task<ApiResult<UserSignUpResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            return _authService.SignUp(request.UserSignUpRequest, cancellationToken);
        }
    }
}
