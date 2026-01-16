namespace CleanArchitecture.Core.Domain.Models.Auth
{
    public record RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
