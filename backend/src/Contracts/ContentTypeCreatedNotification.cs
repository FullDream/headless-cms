using MediatR;

namespace Contracts;

public record ContentTypeCreatedNotification(ContentTypeSchemaSnapshot Schema) : INotification;