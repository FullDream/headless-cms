using BuildingBlocks.Events;
using ContentTypes.Application.Common.ContentField;
using ContentTypes.Application.Common.ContentType;
using ContentTypes.Core;
using ContentTypes.Core.Events;
using Contracts.Notifications;
using MediatR;
using SharedKernel.Events;

namespace ContentTypes.Application.Integration;

internal sealed class ContentTypeMediatorDomainEventPublisher(IMediator mediator)
	: DomainEventPublisherBase<ContentType, INotification>
{
	protected override IEnumerable<IntegrationEvent<INotification>> CollectTyped(
		IEnumerable<IDomainEvent<ContentType>> events)
	{
		foreach (var @event in events)
		{
			switch (@event)
			{
				case ContentTypeCreatedEvent created:
					yield return new IntegrationEvent<INotification>(
						"created",
						new ContentTypeCreatedNotification(created.AggregateRoot.ToSnapshot()));
					break;
				case ContentTypeRenamedEvent renamed:
					yield return new IntegrationEvent<INotification>(
						"renamed",
						new ContentTypeRenamedNotification(renamed.OldName, renamed.AggregateRoot.Name));
					break;
				case ContentTypeRemovedEvent removed:
					yield return new IntegrationEvent<INotification>("removed",
						new ContentTypeRemovedNotification(removed.AggregateRoot.Name));
					break;
				case ContentFieldAddedEvent addedField:
					yield return new IntegrationEvent<INotification>("addedField",
						new ContentFieldAddedNotification(addedField.AggregateRoot.ToSnapshot(),
							addedField.Field.ToDef()));
					break;

				case ContentFieldsRemovedEvent removedField:
					yield return new IntegrationEvent<INotification>("removedField",
						new ContentFieldsRemovedNotification(removedField.AggregateRoot.ToSnapshot(),
							removedField.Field.ToDef()));
					break;
			}
		}
	}

	protected override Task DispatchAsync(
		IntegrationEvent<INotification> ev,
		CancellationToken cancellationToken) =>
		mediator.Publish(ev.Payload, cancellationToken);
}