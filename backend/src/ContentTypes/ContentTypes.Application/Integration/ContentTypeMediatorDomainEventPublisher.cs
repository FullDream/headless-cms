using BuildingBlocks.IntegrationEvents;
using ContentTypes.Application.Common.ContentType;
using ContentTypes.Core;
using ContentTypes.Core.Events;
using Contracts;
using MediatR;
using SharedKernel.Events;

namespace ContentTypes.Application.Integration;

internal sealed class ContentTypeMediatorDomainEventPublisher(IMediator mediator)
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