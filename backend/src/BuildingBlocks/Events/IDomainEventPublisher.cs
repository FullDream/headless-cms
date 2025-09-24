using SharedKernel.Events;

namespace BuildingBlocks.Events;

public abstract record IntegrationEvent(string EventName);

public sealed record IntegrationEvent<TPayload>(string EventName, TPayload Payload)
	: IntegrationEvent(EventName);

public interface IDomainEventPublisher
{
	IEnumerable<IntegrationEvent> Collect(IEnumerable<IDomainEvent> events);

	Task DispatchAsync(IntegrationEvent @event, CancellationToken cancellationToken);
}