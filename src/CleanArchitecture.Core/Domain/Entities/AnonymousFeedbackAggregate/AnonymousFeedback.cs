using CleanArchitecture.Core.Domain.Entities.SubscriptionAggregate;
using CleanArchitecture.Core.Exceptions.Specifics.AnonymousFeedbackExceptions;
using CleanArchitecture.Core.Helper.GaurdClause;

namespace CleanArchitecture.Core.Domain.Entities.AnonymousFeedbackAggregate
{
    public class AnonymousFeedback : EntityBase<int> // Is not the Aggregate Root
    {
        public int AnonymousSubscriptionId { get; private set; } // Foreign Key
        public string? Message { get; private set; }
        public DateTimeOffset CreatedDate { get; private set; }

        // Navigate property
        public AnonymousSubscription AnonymousSubscription { get; private set; } = default!;

        private AnonymousFeedback() { }

        public AnonymousFeedback(AnonymousSubscription subscription, string? message)
        {
            Guard.AgainstNullOrEmpty<AnonymousFeedbackMessageEmptyException>(message);

            AnonymousSubscriptionId = subscription.Id;
            AnonymousSubscription = subscription; 
            Message = message;
            CreatedDate = DateTimeOffset.UtcNow;

            AddDomainEvent(new Events.AnonymousFeedbackCreatedEvent(this));
        }
    }
}
