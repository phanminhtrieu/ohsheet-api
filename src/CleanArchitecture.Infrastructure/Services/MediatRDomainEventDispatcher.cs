using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Infrastructure.Services
{
    public class MediatRDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public MediatRDomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchAndClearEvents(IEnumerable<EntityBase<int>> entitiesWithEvents)
        {
            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToArray();
                entity.ClearDomainEvents();

                foreach (var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent);
                }
            }
        }
    }
}
