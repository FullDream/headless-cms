using SharedKernel.Events;

namespace ContentEntries.Core.Events
{
	public record ContentEntryCreatedEvent(ContentEntry AggregateRoot) : IDomainEvent<ContentEntry>;
}