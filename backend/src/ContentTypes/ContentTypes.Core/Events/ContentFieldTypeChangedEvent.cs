using SharedKernel;
using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public record ContentFieldTypeChangedEvent(ContentType AggregateRoot, string Name, FieldType Type)
	: IDomainEvent<ContentType>;