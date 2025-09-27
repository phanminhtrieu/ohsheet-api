using System.Data;

namespace CleanArchitecture.Core.Domain.Models.Auth
{
    public record UserProfileResponse
    {
        public string? Avatar { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // TODO: Use Enum
    }
}
