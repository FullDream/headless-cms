using SharedKernel.Events;

namespace Core.ContentTypes.Events;

public record ContentTypeRenamedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;