using SharedKernel.Events;

namespace Core.ContentEntries.Events;

public record ContentEntryCreatedEvent(ContentEntry AggregateRoot) : IDomainEvent<ContentEntry>;