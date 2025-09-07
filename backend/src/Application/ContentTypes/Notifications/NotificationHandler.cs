using Application.Abstractions;
using Application.Common.Messaging;
using Application.ContentTypes.Dtos;
using Core.ContentTypes.Events;
using MediatR;

namespace Application.ContentTypes.Notifications;

public class NotificationHandler(IEventDispatcher dispatcher)
	: INotificationHandler<DomainEventNotification<ContentTypeCreatedEvent>>
{
	public async Task Handle(DomainEventNotification<ContentTypeCreatedEvent> notification,
		CancellationToken cancellationToken)
	{
		await dispatcher.DispatchAsync("created",
			new ContentTypeDto(notification.DomainEvent.Id,
				notification.DomainEvent.SystemName,
				notification.DomainEvent.Kind,
				[]),
			cancellationToken);
	}
}