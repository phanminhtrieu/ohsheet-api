using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Domain.Entities.RefreshToken;
using CleanArchitecture.Core.Interfaces.TokenService;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Core.UnitOfWork;
using CleanArchitecture.Shared;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Core.Services.TokenService
{
    public class TokenService(
        AppSettings _appSettings,
        IRefreshTokenRepository _refreshTokenRepository,
        IUnitOfWork _unitOfWork,
        UserManager<ApplicationUser> _userManager) : ITokenService
    {
        public async Task<TokenResult> GenerateToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var roles = await _userManager.GetRolesAsync(user);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expires = DateTime.UtcNow.AddMinutes(_appSettings.Identity.ExpiredTime);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: expires,
                    audience: _appSettings.Identity.Audience,
                    issuer: _appSettings.Identity.Issuer,
                    signingCredentials: credentials
                );

            var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = this.GenerateRefreshToken();

            await this.SaveRefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            return new TokenResult
            {
                UserId = user.Id,
                Token = tokenResult,
                RefreshToken = refreshToken,
                Expires = expires
            };
        }

        public async Task<TokenResult> GenerateToken(ApplicationUser user, string[] scopes, CancellationToken cancellationToken)
        {
            var result = new TokenResult();

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Uri, user?.Avatar ?? "default.png"),
                new Claim("scope", string.Join(" ", scopes))
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(_appSettings.Identity.ExpiredTime);

            var token = new JwtSecurityToken(
                issuer: _appSettings.Identity.Issuer,
                audience: _appSettings.Identity.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = this.GenerateRefreshToken();

            await this.SaveRefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddDays(7));

            result.UserId = user.Id;
            result.Expires = expires;
            result.Token = tokenResult;
            result.RefreshToken = refreshToken;

            return result;
        }

        public async Task<CleanArchitecture.Core.Domain.Entities.RefreshToken.RefreshToken?> ValidateRefreshToken(string token)
        {
            return await _refreshTokenRepository.FindAsync(r => r.Token == token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            IdentityModelEventSource.ShowPII = true;
            TokenValidationParameters validationParameters = new()
            {
                ValidIssuer = _appSettings.Identity.Issuer,
                ValidAudience = _appSettings.Identity.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);

            return principal;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task SaveRefreshToken(Guid userId, string token, DateTime expires)
        {
            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = userId,
                Expires = expires,
                Created = DateTime.UtcNow
            };

            var checkToken = await _refreshTokenRepository.FindAsync(r => r.UserId == userId);

            if (checkToken == null)
            {
                await _refreshTokenRepository.AddAsync(refreshToken);
            }
            else
            {
                checkToken.Token = token;
                checkToken.Expires = expires;
                checkToken.Created = DateTime.UtcNow;
                _refreshTokenRepository.Update(checkToken);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
