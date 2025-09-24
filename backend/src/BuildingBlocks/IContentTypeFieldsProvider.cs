using Contracts;

namespace BuildingBlocks;

public interface IContentTypeFieldsProvider
{
	Task<ContentFieldsSnapshot?> FindByNameAsync(string typeName, CancellationToken ct);
}