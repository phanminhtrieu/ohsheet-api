namespace CleanArchitecture.Core.Domain.Entities
{
    public class EntityBase<T>
    {
        public T Id { get; set; }

        private readonly List<DomainEventBase> _domainEvents = new();

        public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(DomainEventBase domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
