using System.Security.Claims;

namespace CleanArchitecture.Core.Interfaces.UserServices
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        Guid? UserGuid { get; }
        bool IsAuthenticated { get; }
        ClaimsPrincipal? User { get; }
    }
}
