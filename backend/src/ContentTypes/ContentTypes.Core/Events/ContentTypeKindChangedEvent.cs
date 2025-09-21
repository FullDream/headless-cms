using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public record ContentTypeKindChangedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;