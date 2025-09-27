using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using System.Security.Claims;

namespace CleanArchitecture.Core.Interfaces.TokenService
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
        ClaimsPrincipal ValidateToken(string token);
        Task<TokenResult> GenerateToken(ApplicationUser user, string[] scopes, CancellationToken cancellationToken);
    }
}
