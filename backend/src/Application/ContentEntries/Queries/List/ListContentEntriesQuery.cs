using Application.Common.Messaging;

namespace Application.ContentEntries.Queries.List;

public record ListContentEntriesQuery(string ContentTypeName)
	: IQuery<IReadOnlyList<IReadOnlyDictionary<string, object?>>>;