using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public record ContentTypeRenamedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;