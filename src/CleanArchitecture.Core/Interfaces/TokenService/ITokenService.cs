using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using System.Security.Claims;

namespace CleanArchitecture.Core.Interfaces.TokenService
{
    public interface ITokenService
    {
        Task<TokenResult> GenerateToken(ApplicationUser user);
        ClaimsPrincipal ValidateToken(string token);
        Task<TokenResult> GenerateToken(ApplicationUser user, string[] scopes, CancellationToken cancellationToken);
        Task<CleanArchitecture.Core.Domain.Entities.RefreshToken.RefreshToken?> ValidateRefreshToken(string token);
    }
}
