using SharedKernel.Events;

namespace Core.ContentTypes.Events;

public record ContentTypeCreatedEvent(Guid Id, string SystemName, ContentTypeKind Kind)
	: IDomainEvent;