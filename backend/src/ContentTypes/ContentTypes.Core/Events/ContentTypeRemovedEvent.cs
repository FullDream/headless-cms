using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public record ContentTypeRemovedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;