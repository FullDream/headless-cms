using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public record ContentFieldRequiredChangedEvent(ContentType AggregateRoot, string FieldName, bool IsRequired)
	: IDomainEvent<ContentType>;