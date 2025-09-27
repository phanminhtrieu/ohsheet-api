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
        public string GenerateToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.Role, user.Role.ToString()), // TODO: Add role
            };

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    audience: _appSettings.Identity.Audience,
                    issuer: _appSettings.Identity.Issuer,
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<TokenResult> GenerateToken(ApplicationUser user, string[] scopes, CancellationToken cancellationToken)
        {
            var result = new TokenResult();

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Uri, user?.Avatar ?? "default.png"),
            //new Claim(ClaimTypes.Role, roles == null ? Role.User.ToString() : string.Join(";", roles)), // TODO: Add role
            new Claim("scope", string.Join(" ", scopes)) // Adding scope claim
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddDays(_appSettings.Identity.ExpiredTime);

            var token = new JwtSecurityToken(
                issuer: _appSettings.Identity.Issuer,
                audience: _appSettings.Identity.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);

            result.UserId = user.Id;
            result.Expires = expires;
            result.Token = tokenResult;

            var refreshToken = new RefreshToken
            {
                Token = tokenResult,
                UserId = user.Id,
                Expires = expires,
                Created = DateTime.UtcNow
            };
            var checkToken = await _refreshTokenRepository.FindAsync(r => r.UserId == user.Id);

            var isRefreshTokenExist = checkToken != null;
            var isRefreshTokenValid = isRefreshTokenExist && (checkToken.Expires > DateTime.UtcNow);

            // TODO: Use switch case
            if (!isRefreshTokenExist)
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
                try
                {
                    await _refreshTokenRepository.AddAsync(refreshToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
            else if (isRefreshTokenValid)
            {
                checkToken.Token = tokenResult;
                checkToken.Expires = expires;
                checkToken.Created = DateTime.UtcNow;

                await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
                try
                {
                    _refreshTokenRepository.Update(refreshToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }

            }
            // If refresh token is exist and expired, then delete it and add new one
            else
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
                try
                {
                    _refreshTokenRepository.Remove(refreshToken);
                    await _refreshTokenRepository.AddAsync(refreshToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }

            return result;
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            IdentityModelEventSource.ShowPII = true;
            TokenValidationParameters validationParameters = new()
            {
                ValidIssuer = _appSettings.Identity.Issuer,
                ValidAudience = _appSettings.Identity.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Identity.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);

            return principal;
        }
    }
}
