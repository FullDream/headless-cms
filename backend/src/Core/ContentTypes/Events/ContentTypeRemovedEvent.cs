using SharedKernel.Events;

namespace Core.ContentTypes.Events;

public record ContentTypeRemovedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;