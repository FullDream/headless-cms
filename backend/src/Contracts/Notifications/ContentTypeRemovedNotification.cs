using MediatR;

namespace Contracts.Notifications;

public record ContentTypeRemovedNotification(string ContentTypeName) : INotification;