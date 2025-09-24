using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public sealed record ContentTypeRemovedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;