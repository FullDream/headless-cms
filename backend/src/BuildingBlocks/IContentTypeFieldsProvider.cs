using Contracts;

namespace BuildingBlocks;

public interface IContentTypeFieldsProvider
{
	Task<ContentTypeSchemaSnapshot?> FindByNameAsync(string typeName, CancellationToken ct);
}