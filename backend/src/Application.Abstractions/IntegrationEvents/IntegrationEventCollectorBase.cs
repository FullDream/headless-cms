using SharedKernel;
using SharedKernel.Events;

namespace Application.Abstractions.IntegrationEvents;

public abstract class IntegrationEventCollectorBase<TRoot> : IIntegrationEventCollector where TRoot : AggregateRoot
{
	public IEnumerable<(string EventName, object Payload)> Collect(IEnumerable<IDomainEvent> events)
		=> CollectTyped(events.OfType<IDomainEvent<TRoot>>());

	public abstract Task DispatchAsync(string eventName, object payload, CancellationToken cancellationToken);

	protected abstract IEnumerable<(string EventName, object Payload)> CollectTyped(
		IEnumerable<IDomainEvent<TRoot>> events);
}