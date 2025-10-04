using MediatR;

namespace Contracts.Notifications;

public record ContentFieldAddedNotification(ContentTypeSchemaSnapshot ContentTypeSnapshot, ContentFieldDef Field)
	: INotification;