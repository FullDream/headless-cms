using SharedKernel.Events;

namespace Application.Abstractions;

public interface IDomainEventPublisher
{
	Task PublishAsync(IReadOnlyCollection<IDomainEvent> @events, CancellationToken cancellationToken = default);
}