using BuildingBlocks.Messaging;

namespace ContentEntries.Application.List;

public record ListContentEntriesQuery(string ContentTypeName)
	: IQuery<IReadOnlyList<IReadOnlyDictionary<string, object?>>>;