using ContentEntries.Application.Mappers;
using ContentEntries.Core;
using MediatR;
using SharedKernel.Result;

namespace ContentEntries.Application.Queries.List;

public class ListContentEntriesHandler(
	IContentEntryRepository repository)
	: IRequestHandler<ListContentEntriesQuery, Result<IReadOnlyList<IReadOnlyDictionary<string, object?>>>>
{
	public async Task<Result<IReadOnlyList<IReadOnlyDictionary<string, object?>>>> Handle(
		ListContentEntriesQuery request,
		CancellationToken ct)
	{
		var items = await repository.QueryAsync(request.ContentTypeName, ct);

		if (items is null)
			throw new InvalidOperationException("No content entries found");

		return items.Select(item => item.ToDto()).ToList().AsReadOnly();
	}
}