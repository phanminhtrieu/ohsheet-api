using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.Infrastructure.Data
{
    public class EventDispatchInterceptor : SaveChangesInterceptor
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public EventDispatchInterceptor(IDomainEventDispatcher domainEventDispatcher)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        // Called after SaveChangesAsync has completed successfully
        public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
          CancellationToken cancellationToken = new CancellationToken())
        {
            if (eventData.Context is null)
                return await base.SavedChangesAsync(eventData, result, cancellationToken);

            // Retrieve all tracked entities that have domain events
            var entitiesWithEvents = eventData.Context.ChangeTracker
                .Entries<EntityBase<int>>() // TODO: Make generic
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            // Dispatch and clear domain events
            await _domainEventDispatcher.DispatchAndClearEvents(entitiesWithEvents);

            return await base.SavedChangesAsync(eventData, result, cancellationToken);

        }
    }

}
