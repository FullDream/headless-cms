using ContentEntries.Application.Abstractions;
using Contracts;
using MediatR;

namespace ContentEntries.Application.ContentTypeCreated;

internal sealed class ContentTypeCreatedHandler(IContentSchemaManager schemaManager)
	: INotificationHandler<ContentTypeCreatedNotification>
{
	public async Task Handle(ContentTypeCreatedNotification notification, CancellationToken cancellationToken)
	{
		await schemaManager.EnsureStructureCreatedAsync(notification.Schema, cancellationToken);
	}
}