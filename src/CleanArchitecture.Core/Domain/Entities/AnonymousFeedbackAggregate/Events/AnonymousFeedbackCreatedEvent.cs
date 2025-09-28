using CleanArchitecture.Core.Common.Constants;
using CleanArchitecture.Core.Interfaces.MailServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.Domain.Entities.AnonymousFeedbackAggregate.Events
{
    internal sealed class AnonymousFeedbackCreatedEvent : DomainEventBase
    {
        public AnonymousFeedback AnonymousFeedback { get; }

        public AnonymousFeedbackCreatedEvent(AnonymousFeedback feedback)
        {
            AnonymousFeedback = feedback;
        }
    }

    internal class AnonymousFeedbackCreatedEventHandler(ILogger<AnonymousFeedbackCreatedEventHandler> _logger, IEmailService _emailService) : INotificationHandler<AnonymousFeedbackCreatedEvent>
    {
        public async Task Handle(AnonymousFeedbackCreatedEvent notification, CancellationToken cancellationToken)
        {
            var message = 
                $"Feedback: {notification.AnonymousFeedback.Message}, /n" +
                $"SubscriptionId: {notification.AnonymousFeedback.AnonymousSubscriptionId}, /n" +
                $"Name: {notification.AnonymousFeedback.AnonymousSubscription.Name}, /n" + 
                $"Email: {notification.AnonymousFeedback.AnonymousSubscription.Name},";

            _logger.LogInformation($"Anonymous Feedback Created event: {notification.AnonymousFeedback.Id} - SubscriptionId: {notification.AnonymousFeedback.AnonymousSubscriptionId}");



            // Send an email to admin
            await _emailService.SendEmailAsync("admin@gmail.com", EmailSubjects.HAVE_NEW_FEEDBACK, message);
        }
    }
}
