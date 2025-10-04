using ContentEntries.Application.Abstractions;
using Contracts.Notifications;
using MediatR;

namespace ContentEntries.Application.ContentTypeCreated;

internal sealed class ContentTypeCreatedHandler(IContentSchemaManager schemaManager)
	: INotificationHandler<ContentTypeCreatedNotification>
{
	public Task Handle(ContentTypeCreatedNotification notification, CancellationToken cancellationToken) =>
		schemaManager.EnsureStructureCreatedAsync(notification.Schema, cancellationToken);
}