namespace CleanArchitecture.Core.Domain.Models.Auth
{
    public record UserSignInRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
