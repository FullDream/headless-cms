using MediatR;

namespace BuildingBlocks.Notifications;

public record ContentTypeRenamedNotification(string OldName, string NewName) : INotification;