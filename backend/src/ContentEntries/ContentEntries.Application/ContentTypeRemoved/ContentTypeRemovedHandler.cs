using ContentEntries.Application.Abstractions;
using Contracts.Notifications;
using MediatR;

namespace ContentEntries.Application.ContentTypeRemoved;

internal sealed class ContentTypeRemovedHandler(IContentSchemaManager schemaManager)
	: INotificationHandler<ContentTypeRemovedNotification>
{
	public Task Handle(ContentTypeRemovedNotification notification, CancellationToken cancellationToken) =>
		schemaManager.RemoveStructureAsync(notification.ContentTypeName, cancellationToken);
}