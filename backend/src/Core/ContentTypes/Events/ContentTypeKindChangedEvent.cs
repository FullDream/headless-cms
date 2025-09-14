using SharedKernel.Events;

namespace Core.ContentTypes.Events;

public record ContentTypeKindChangedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;