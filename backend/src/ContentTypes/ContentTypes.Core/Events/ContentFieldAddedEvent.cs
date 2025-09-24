using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public record ContentFieldAddedEvent(ContentType AggregateRoot, ContentField Field) : IDomainEvent<ContentType>;