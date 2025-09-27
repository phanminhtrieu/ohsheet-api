namespace CleanArchitecture.Core.Domain.Entities.AuditLogin
{
    public class AuditLogin : EntityBase<int>
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserAgent { get; set; }
        public string? Domain { get; set; }
        public string? IpAddress { get; set; }
        public string? Url { get; set; }
        public string? Notes { get; set; }
        public bool IsSuccessded { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
