using CleanArchitecture.Core.Common.Constants;
using CleanArchitecture.Core.Interfaces.MailServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.Domain.Entities.SubscriptionAggregate.Events
{
    internal sealed class AnonymousSubscriptionCreatedEvent : DomainEventBase
    {
        public AnonymousSubscription Subscription { get; }
        public AnonymousSubscriptionCreatedEvent(AnonymousSubscription subscription)
        {
            Subscription = subscription;
        }
    }

    internal class AnonymousSubscriptionCreatedEventHandler(ILogger<AnonymousSubscriptionCreatedEventHandler> _logger,
        IEmailService _emailService) : INotificationHandler<AnonymousSubscriptionCreatedEvent>
    {
        public async Task Handle(AnonymousSubscriptionCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Subscription Created event: {notification.Subscription.Id} - {notification.Subscription.Email}");

            var message = string.Format(EmailHtmlMessages.WELCOME, notification.Subscription.Name);

            // Send an email
            await _emailService.SendEmailAsync(
                notification.Subscription.Email.Value, 
                EmailSubjects.WELCOME, 
                message);
        }
    }
}
