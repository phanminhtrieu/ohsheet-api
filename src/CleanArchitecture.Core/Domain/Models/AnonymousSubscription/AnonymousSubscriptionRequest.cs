namespace CleanArchitecture.Core.Domain.Models.AnonymousSubscription
{
    public class AnonymousSubscriptionRequest
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Message { get; set; }
    }
}
