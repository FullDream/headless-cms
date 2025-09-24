using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public sealed record ContentTypeCreatedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;