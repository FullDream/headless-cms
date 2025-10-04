using MediatR;

namespace Contracts.Notifications;

public sealed record ContentFieldsRemovedNotification(
	ContentTypeSchemaSnapshot ContentTypeSnapshot,
	ContentFieldDef Field)
	: INotification;