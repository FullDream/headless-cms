using SharedKernel.Events;

namespace BuildingBlocks;

public interface IDomainEventPublisher
{
	Task PublishAsync(IReadOnlyCollection<IDomainEvent> @events, CancellationToken cancellationToken = default);
}