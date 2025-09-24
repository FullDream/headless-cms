using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public sealed record ContentTypeKindChangedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;