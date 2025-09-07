using Application.Abstractions;
using MediatR;
using SharedKernel.Events;

namespace Application.Common.Messaging;

public class MediatorDomainEventPublisher(IMediator mediator) : IDomainEventPublisher
{
	public async Task PublishAsync(IReadOnlyCollection<IDomainEvent> @events,
		CancellationToken cancellationToken = default)
	{
		foreach (var @event in @events)
		{
			var eventType = @event.GetType();
			var notificationType = typeof(DomainEventNotification<>).MakeGenericType(eventType);
			var notification = Activator.CreateInstance(notificationType, @event);

			if (notification is null) continue;

			await mediator.Publish((INotification)notification, cancellationToken);
		}
	}
}