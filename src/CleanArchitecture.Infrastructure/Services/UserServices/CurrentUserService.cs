using CleanArchitecture.Core.Interfaces.CookieServices;
using CleanArchitecture.Core.Interfaces.TokenService;
using CleanArchitecture.Core.Interfaces.UserServices;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CleanArchitecture.Infrastructure.Services.UserServices
{
    public class CurrentUserService(
        IHttpContextAccessor _httpContextAccessor,
        ICookieService _cookieService,
        ITokenService _tokenService) : ICurrentUserService
    {
        private ClaimsPrincipal? _user;

        public ClaimsPrincipal? User
        {
            get
            {
                if (_user != null) return _user;

                // 1. Try get from HttpContext
                var contextUser = _httpContextAccessor.HttpContext?.User;
                if (contextUser?.Identity?.IsAuthenticated == true)
                {
                    _user = contextUser;
                    return _user;
                }

                // 2. Fallback to Cookie
                var token = _cookieService.Get();
                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        _user = _tokenService.ValidateToken(token);
                    }
                    catch
                    {
                        // Log or handle validation failure if needed
                    }
                }

                return _user;
            }
        }

        public string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public Guid? UserGuid => Guid.TryParse(UserId, out var guid) ? guid : null;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    }
}
