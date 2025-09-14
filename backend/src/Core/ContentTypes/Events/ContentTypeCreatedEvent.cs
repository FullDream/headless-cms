using SharedKernel.Events;

namespace Core.ContentTypes.Events;

public record ContentTypeCreatedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;