using SharedKernel.Events;

namespace Core.ContentTypes.Events;

public record ContentFieldsAddedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;