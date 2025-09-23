using MediatR;

namespace Application.Abstractions.Notifications;

public record ContentTypeCreatedNotification(ContentFieldsSnapshot Schema) : INotification;