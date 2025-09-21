using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public record ContentFieldsRemovedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;