using MediatR;

namespace Contracts.Notifications;

public record ContentTypeRenamedNotification(string OldName, string NewName) : INotification;