namespace CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results
{
    public class TokenResult
    {
        public Guid UserId { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime Expires { get; set; }
    }
}
