using SharedKernel.Events;

namespace Core.ContentTypes.Events;

public record ContentTypeRenamedEvent(Guid Id, string Name) : IDomainEvent;