using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public record ContentFieldsAddedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;