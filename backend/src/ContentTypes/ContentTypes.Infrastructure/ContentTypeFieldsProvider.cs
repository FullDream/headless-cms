using BuildingBlocks;
using ContentTypes.Core;
using ContentTypes.Infrastructure.Mappers;
using Contracts;

namespace ContentTypes.Infrastructure;

internal sealed class ContentTypeFieldsProvider(IContentTypeRepository contentTypeRepository)
	: IContentTypeFieldsProvider
{
	public async Task<ContentTypeSchemaSnapshot?> FindByNameAsync(string typeName, CancellationToken ct)
	{
		var schema = await contentTypeRepository.FindByNameAsync(typeName, ct);

		return schema?.ToSnapshot();
	}
}