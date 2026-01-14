namespace CleanArchitecture.Core.Domain.Models.Auth
{
    public record UserProfileDto
    {
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}
