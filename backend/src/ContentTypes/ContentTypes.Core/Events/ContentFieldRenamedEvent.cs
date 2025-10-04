using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public record ContentFieldRenamedEvent(ContentType AggregateRoot, string Name, string OldName)
	: IDomainEvent<ContentType>;