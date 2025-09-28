using MediatR;

namespace Contracts.Notifications;

public record ContentTypeCreatedNotification(ContentTypeSchemaSnapshot Schema) : INotification;