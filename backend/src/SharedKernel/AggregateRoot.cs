using SharedKernel.Events;

namespace SharedKernel;

public abstract class AggregateRoot
{
	private readonly List<IDomainEvent> domainEvents = [];
	public IReadOnlyCollection<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();

	public void ClearDomainEvents() => domainEvents.Clear();

	protected void AddDomainEvent(IDomainEvent @event) => domainEvents.Add(@event);
}