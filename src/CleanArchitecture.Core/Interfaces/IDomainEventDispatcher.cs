using CleanArchitecture.Core.Domain.Entities;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAndClearEvents(IEnumerable<EntityBase<int>> aggregatesWithEvents);
    }
}
