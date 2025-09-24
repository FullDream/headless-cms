using SharedKernel;
using SharedKernel.Events;

namespace BuildingBlocks.IntegrationEvents;

public abstract class DomainEventPublisherBase<TRoot, TPayload>
	: IDomainEventPublisher
	where TRoot : AggregateRoot
{
	public IEnumerable<IntegrationEvent> Collect(IEnumerable<IDomainEvent> events) =>
		CollectTyped(events.OfType<IDomainEvent<TRoot>>());

	public async Task DispatchAsync(IntegrationEvent @event, CancellationToken cancellationToken)
	{
		if (@event is IntegrationEvent<TPayload> typed)
		{
			await DispatchAsync(typed, cancellationToken);
		}
		else
		{
			throw new InvalidOperationException(
				$"Unexpected payload type for {GetType().Name}: {@event.GetType().Name}");
		}
	}

	protected abstract IEnumerable<IntegrationEvent<TPayload>> CollectTyped(
		IEnumerable<IDomainEvent<TRoot>> events);

	protected abstract Task DispatchAsync(IntegrationEvent<TPayload> ev, CancellationToken cancellationToken);
}