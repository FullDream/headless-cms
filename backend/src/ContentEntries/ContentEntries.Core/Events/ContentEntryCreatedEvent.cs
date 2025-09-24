using SharedKernel.Events;

namespace ContentEntries.Core.Events
{
	public sealed record ContentEntryCreatedEvent(ContentEntry AggregateRoot) : IDomainEvent<ContentEntry>;
}