using BuildingBlocks.Messaging;

namespace ContentEntries.Application.Queries.List;

public record ListContentEntriesQuery(string ContentTypeName)
	: IQuery<IReadOnlyList<IReadOnlyDictionary<string, object?>>>;