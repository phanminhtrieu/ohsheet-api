using CleanArchitecture.Core.Domain.Models.Auth;
using CleanArchitecture.Core.Interfaces.AuthServices;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using MediatR;

namespace CleanArchitecture.UseCases.Auth
{
    public record RefreshTokenCommand(RefreshTokenRequest RefreshTokenRequest) : IRequest<ApiResult<UserSignInResponse>> { }

    public class RefreshTokenHandler(IAuthService _authService) : IRequestHandler<RefreshTokenCommand, ApiResult<UserSignInResponse>>
    {
        public async Task<ApiResult<UserSignInResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RefreshToken(request.RefreshTokenRequest.RefreshToken);
        }
    }
}
