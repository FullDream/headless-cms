using Application.Abstractions;
using Application.Abstractions.Notifications;
using MediatR;

namespace ContentEntries.Application.ContentTypeCreated;

public class ContentTypeCreatedHandler(IContentSchemaManager schemaManager)
	: INotificationHandler<ContentTypeCreatedNotification>
{
	public async Task Handle(ContentTypeCreatedNotification notification, CancellationToken cancellationToken)
	{
		await schemaManager.EnsureStructureCreatedAsync(notification.Schema, cancellationToken);
	}
}