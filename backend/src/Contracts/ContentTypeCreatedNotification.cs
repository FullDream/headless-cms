using MediatR;

namespace Contracts;

public record ContentTypeCreatedNotification(ContentFieldsSnapshot Schema) : INotification;