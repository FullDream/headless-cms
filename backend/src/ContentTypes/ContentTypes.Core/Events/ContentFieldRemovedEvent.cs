using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public sealed record ContentFieldsRemovedEvent(ContentType AggregateRoot, ContentField Field)
	: IDomainEvent<ContentType>;