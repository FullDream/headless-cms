using Application.Abstractions;
using Application.ContentEntries.Mappers;
using Core.ContentEntries;
using MediatR;

namespace Application.ContentEntries.Queries.List;

public class ListContentEntriesHandler(
	IContentEntryRepository repository,
	IContentTypeFieldsProvider contentTypeFieldsProvider)
	: IRequestHandler<ListContentEntriesQuery, IReadOnlyList<IReadOnlyDictionary<string, object?>>>
{
	public async Task<IReadOnlyList<IReadOnlyDictionary<string, object?>>> Handle(ListContentEntriesQuery request,
		CancellationToken ct)
	{
		var items = await repository.QueryAsync(request.ContentTypeName, ct);

		if (items is null)
			throw new InvalidOperationException("No content entries found");

		return items.Select(item => item.ToDto()).ToList().AsReadOnly();
	}
}