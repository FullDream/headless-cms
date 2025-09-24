using SharedKernel.Events;

namespace ContentTypes.Core.Events;

public sealed record ContentTypeRenamedEvent(ContentType AggregateRoot, string OldName) : IDomainEvent<ContentType>;