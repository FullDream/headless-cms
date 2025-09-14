using SharedKernel.Events;

namespace Core.ContentTypes.Events;

public record ContentFieldsRemovedEvent(ContentType AggregateRoot) : IDomainEvent<ContentType>;