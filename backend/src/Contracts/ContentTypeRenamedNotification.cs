using MediatR;

namespace Contracts;

public record ContentTypeRenamedNotification(string OldName, string NewName) : INotification;