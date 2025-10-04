using MediatR;

namespace Contracts.Notifications;

public record ContentFieldRequiredChangedNotification(string FieldName, bool IsRequired) : INotification;