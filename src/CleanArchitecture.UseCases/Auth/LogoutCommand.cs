using CleanArchitecture.Core.Interfaces.AuthServices;
using MediatR;

namespace CleanArchitecture.UseCases.Auth
{
    public record LogoutCommand() : IRequest<bool> { }

    public class LogoutHandler(IAuthService _authService) : IRequestHandler<LogoutCommand, bool>
    {
        public Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_authService.Logout()); 
        }
    }
}

