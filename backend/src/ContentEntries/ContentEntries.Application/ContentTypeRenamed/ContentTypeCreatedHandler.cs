using ContentEntries.Application.Abstractions;
using Contracts.Notifications;
using MediatR;

namespace ContentEntries.Application.ContentTypeRenamed;

internal sealed class ContentTypeRenamedHandler(IContentSchemaManager schemaManager)
	: INotificationHandler<ContentTypeRenamedNotification>
{
	public Task Handle(ContentTypeRenamedNotification notification, CancellationToken cancellationToken) =>
		schemaManager.RenameStructureAsync(notification.OldName, notification.NewName, cancellationToken);
}