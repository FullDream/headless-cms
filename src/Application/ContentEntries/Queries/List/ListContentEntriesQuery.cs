using MediatR;

namespace Application.ContentEntries.Queries.List;

public record ListContentEntriesQuery(string ContentTypeName)
	: IRequest<IReadOnlyList<IReadOnlyDictionary<string, object?>>>;