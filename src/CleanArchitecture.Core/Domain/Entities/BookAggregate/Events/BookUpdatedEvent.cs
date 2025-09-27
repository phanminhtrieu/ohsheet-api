using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.Domain.Entities.BookAggregate.Events
{
    internal sealed class BookUpdatedEvent : DomainEventBase
    {
        public int BookId { get; }

        public BookUpdatedEvent(Book book)
        {
            BookId = book.Id;
        }
    }

    internal class BookUpdatedEventHandler(ILogger<BookUpdatedEventHandler> _logger) : INotificationHandler<BookUpdatedEvent>
    {
        public Task Handle(BookUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Book Updated event: {notification.BookId}");

            // Send an email or something else ...

            return Task.CompletedTask;
        }
    }
}
