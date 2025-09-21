using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public record ContentTypeCreatedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;