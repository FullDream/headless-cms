using BuildingBlocks;
using ContentTypes.Core;
using ContentTypes.Infrastructure.Mappers;

namespace ContentTypes.Infrastructure;

internal sealed class ContentTypeFieldsProvider(IContentTypeRepository contentTypeRepository)
	: IContentTypeFieldsProvider
{
	public async Task<ContentFieldsSnapshot?> FindByNameAsync(string typeName, CancellationToken ct)
	{
		var schema = await contentTypeRepository.FindByNameAsync(typeName, ct);

		return schema?.ToSnapshot();
	}
}