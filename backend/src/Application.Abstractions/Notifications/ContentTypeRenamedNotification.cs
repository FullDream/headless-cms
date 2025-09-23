using MediatR;

namespace Application.Abstractions.Notifications;

public record ContentTypeRenamedNotification(string OldName, string NewName) : INotification;