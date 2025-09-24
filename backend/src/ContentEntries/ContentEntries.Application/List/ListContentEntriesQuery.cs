using BuildingBlocks.Messaging;

namespace ContentEntries.Application.List;

public sealed record ListContentEntriesQuery(string ContentTypeName)
	: IQuery<IReadOnlyList<IReadOnlyDictionary<string, object?>>>;