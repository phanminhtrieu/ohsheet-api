namespace CleanArchitecture.Core.Domain.Models.AuditLogins
{
    public record AuditLoginRequest
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Notes { get; set; }
        public bool IsSuccessded { get; set; }
    }
}
