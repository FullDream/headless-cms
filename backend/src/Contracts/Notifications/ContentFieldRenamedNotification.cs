using MediatR;

namespace Contracts.Notifications;

public record ContentFieldRenamedNotification(string OldName, string NewName) : INotification;