using CleanArchitecture.Core.Interfaces.CookieServices;
using CleanArchitecture.Shared.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Core.Services.CookieServices
{
    public class CookieService(IHttpContextAccessor _httpContextAccessor) : ICookieService
    {
        public string? Get()
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies["token_key"];
        }

        public void Set(string token)
            => _httpContextAccessor.HttpContext?.Response.Cookies.Append("token_key", token,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Lax, // Changed from None to Lax for HTTP compatibility
                        Secure = false, // Changed from true to false for HTTP development
                        MaxAge = TimeSpan.FromMinutes(30),
                        Path = "/"
                    });

        public void Delete() => _httpContextAccessor.HttpContext?.Response.Cookies.Delete("token_key");
    }
}
