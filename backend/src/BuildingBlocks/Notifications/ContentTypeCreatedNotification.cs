using MediatR;

namespace BuildingBlocks.Notifications;

public record ContentTypeCreatedNotification(ContentFieldsSnapshot Schema) : INotification;