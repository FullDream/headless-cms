using ContentEntries.Application.Abstractions;
using Contracts.Notifications;
using MediatR;

namespace ContentEntries.Application.ContentFieldAdded;

internal sealed class ContentFieldAddedHandler(IContentSchemaManager schemaManager)
	: INotificationHandler<ContentFieldAddedNotification>
{
	public Task Handle(ContentFieldAddedNotification notification, CancellationToken cancellationToken) =>
		schemaManager.AddFieldToStructureAsync(notification.ContentTypeSnapshot,
			notification.Field,
			cancellationToken);
}