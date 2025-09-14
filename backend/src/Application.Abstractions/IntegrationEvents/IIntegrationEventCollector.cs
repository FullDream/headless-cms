using SharedKernel.Events;

namespace Application.Abstractions.IntegrationEvents;

public interface IIntegrationEventCollector
{
	IEnumerable<(string EventName, object Payload)> Collect(IEnumerable<IDomainEvent> events);

	Task DispatchAsync(string eventName, object payload, CancellationToken cancellationToken);
}