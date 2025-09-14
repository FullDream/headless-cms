namespace SharedKernel.Events;

public interface IDomainEvent;

public interface IDomainEvent<TRoot> : IDomainEvent
{
	TRoot AggregateRoot { get; }
}