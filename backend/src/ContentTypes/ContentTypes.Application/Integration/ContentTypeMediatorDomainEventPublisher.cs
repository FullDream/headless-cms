using BuildingBlocks.IntegrationEvents;
using BuildingBlocks.Notifications;
using ContentTypes.Application.Mappers;
using ContentTypes.Core;
using ContentTypes.Core.Events;
using MediatR;
using SharedKernel.Events;

namespace ContentTypes.Application.Integration;

public class ContentTypeMediatorDomainEventPublisher(IMediator mediator)
	: DomainEventPublisherBase<ContentType, INotification>
{
	protected override IEnumerable<IntegrationEvent<INotification>> CollectTyped(
		IEnumerable<IDomainEvent<ContentType>> events)
	{
		foreach (var ev in events)
		{
			if (ev is ContentTypeRenamedEvent renamed)
				yield return new IntegrationEvent<INotification>(
					"renamed",
					new ContentTypeRenamedNotification(renamed.OldName, renamed.AggregateRoot.Name));

			if (ev is ContentTypeCreatedEvent created)
				yield return new IntegrationEvent<INotification>(
					"created",
					new ContentTypeCreatedNotification(created.AggregateRoot.ToSnapshot()));
			;
		}
	}

	protected override Task DispatchAsync(
		IntegrationEvent<INotification> ev,
		CancellationToken cancellationToken) =>
		mediator.Publish(ev.Payload, cancellationToken);
}