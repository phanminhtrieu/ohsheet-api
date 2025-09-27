using System.Data;

namespace CleanArchitecture.Core.Domain.Models.Auth
{
    public record UserSignInResponse
    {
        //public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // TODO: Use Enum
        public string Token { get; set; } = string.Empty;
        public int AuditLoginId { get; set; }
    }
}
