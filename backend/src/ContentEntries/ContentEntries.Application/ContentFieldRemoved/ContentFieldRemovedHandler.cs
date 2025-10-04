using ContentEntries.Application.Abstractions;
using Contracts.Notifications;
using MediatR;

namespace ContentEntries.Application.ContentFieldRemoved;

internal sealed class ContentFieldRemovedHandler(IContentSchemaManager schemaManager)
	: INotificationHandler<ContentFieldsRemovedNotification>
{
	public Task Handle(ContentFieldsRemovedNotification notification, CancellationToken cancellationToken) =>
		schemaManager.RemoveFieldFromStructureAsync(notification.ContentTypeSnapshot,
			notification.Field,
			cancellationToken);
}