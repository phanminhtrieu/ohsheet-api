using CleanArchitecture.Core.Common.Constants;
using CleanArchitecture.Core.Domain.Entities.SubscriptionAggregate.Events;
using CleanArchitecture.Core.Interfaces.MailServices;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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

    internal class AnonymousFeedbackCreatedEventHandler(
        ILogger<AnonymousFeedbackCreatedEventHandler> _logger,
        IEmailService _emailService) : INotificationHandler<AnonymousFeedbackCreatedEvent>
    {
        public async Task Handle(AnonymousFeedbackCreatedEvent notification, CancellationToken cancellationToken)
        {
            var message = 
                $"Feedback: {notification.AnonymousFeedback.Message}, " +
                $"SubscriptionId: {notification.AnonymousFeedback.AnonymousSubscriptionId}, " +
                $"Name: {notification.AnonymousFeedback.AnonymousSubscription.Name}, " + 
                $"Email: {notification.AnonymousFeedback.AnonymousSubscription.Email.Value},";

            // Send an email to admin
            _ = Task.Run(() => _emailService.SendEmailAsync("admin@gmail.com", EmailSubjects.HAVE_NEW_FEEDBACK, message)); // TODO: remove fire and forget
        }
    }
}
