using CleanArchitecture.Core.Interfaces.MailServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.Domain.Entities.BookAggregate.Events
{
    /// <summary>
    /// A domain event that is dispatched whenever a contributor is created.
    /// The CreateBookService is used to dispatch this event.
    /// </summary>
    internal sealed class BookCreatedEvent : DomainEventBase
    {
        public Book Book { get; }

        public BookCreatedEvent(Book book)
        {
            Book = book;
        }
    }

    /// <summary>
    /// Handler of BookCreatedEvent
    /// </summary>
    internal class BookCreatedEventHandler : INotificationHandler<BookCreatedEvent>
    {
        private readonly ILogger<BookCreatedEventHandler> _logger;
        private readonly IEmailService _emailService;

        public BookCreatedEventHandler(ILogger<BookCreatedEventHandler> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public Task Handle(BookCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Book Created event: {notification.Book.Id} - {notification.Book.Title} - {notification.Book.Status}");

            // Send an email
            _emailService.SendEmailAsync(
                "concobebe@gmail.com", 
                "Book Created", 
                $"Book Created event: {notification.Book.Id} - {notification.Book.Title} - {notification.Book.Status}");

            return Task.CompletedTask;
        }
    }
}
