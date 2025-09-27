using CleanArchitecture.Core.Interfaces.CookieServices;
using CleanArchitecture.Shared.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Core.Services.CookieServices
{
    public class CookieService(IHttpContextAccessor _httpContextAccessor) : ICookieService
    {
        public string Get()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["token_key"];
            return string.IsNullOrEmpty(token) ? throw UserException.UserUnauthorizedException() : token;
        }

        public void Set(string token)
            => _httpContextAccessor.HttpContext?.Response.Cookies.Append("token_key", token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        MaxAge = TimeSpan.FromMinutes(30)
                    });

        public void Delete() => _httpContextAccessor.HttpContext?.Response.Cookies.Delete("token_key");
    }
}
