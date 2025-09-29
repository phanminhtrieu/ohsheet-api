using CleanArchitecture.Core.Domain.Entities.AnonymousFeedbackAggregate;
using CleanArchitecture.Core.Domain.Entities.SubscriptionAggregate.Events;
using CleanArchitecture.Core.Domain.Interfaces;
using CleanArchitecture.Core.Exceptions.Specifics.AnonymousSubscriptionExceptions;
using CleanArchitecture.Core.Helper.GaurdClause;
using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Domain.Entities.SubscriptionAggregate
{
    public class AnonymousSubscription : EntityBase<int>, IAggregateRoot, IHasDateTracking
    {
        public AnonymousSubscriptionEmail Email { get; private set; }
        public string? Name { get; private set; }
        public DateTimeOffset CreatedDate { get; private set; }
        public DateTimeOffset ModifiedDate { get; private set; }

        // Navigation property 1 - N with AnonymousFeedback
        private readonly List<AnonymousFeedback> _feedback = new();
        public IReadOnlyCollection<AnonymousFeedback> Feedbacks => _feedback.AsReadOnly();

        private AnonymousSubscription() { }

        public AnonymousSubscription(string? email, string? name) 
        { 
            Guard.AgainstNullOrEmpty<AnonymousSubscriptionNameEmptyException>(name);

            Email = new AnonymousSubscriptionEmail(email!); // Guard Clause inside
            Name = name;
            CreatedDate = DateTimeOffset.UtcNow;
            ModifiedDate = DateTimeOffset.UtcNow;
        }
        
        public static AnonymousSubscription Create(string email, string name)
        {
            var subscription = new AnonymousSubscription(email, name);

            subscription.AddDomainEvent(new AnonymousSubscriptionCreatedEvent(subscription));

            return subscription;
        }

        public AnonymousFeedback AddFeedBack(string? message)
        {
            var feedback = new AnonymousFeedback(this, message);
            _feedback.Add(feedback);

            return feedback;
        }
    }

    [Owned]
    public record AnonymousSubscriptionEmail : IValueObject
    {
        [Column("Email")]
        [EmailAddress]
        public string Value { get; set; }

        private AnonymousSubscriptionEmail() { }

        public AnonymousSubscriptionEmail(string email) 
        {
            Guard.AgainstNullOrEmpty<AnonymousSubscriptionEmailEmptyException>(email);
            Guard.AgainstInvalidEmail(email,() => new AnonymousSubscriptionEmailInvalidException(email));

            Value = email;
        }
    }
}
