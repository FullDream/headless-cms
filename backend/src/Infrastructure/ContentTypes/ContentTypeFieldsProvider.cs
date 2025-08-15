using Application.Abstractions;
using Core.ContentTypes;
using Infrastructure.ContentTypes.Mappers;

namespace Infrastructure.ContentTypes;

internal sealed class ContentTypeFieldsProvider(IContentTypeRepository contentTypeRepository)
	: IContentTypeFieldsProvider
{
	public async Task<ContentFieldsSnapshot?> FindByNameAsync(string typeName, CancellationToken ct)
	{
		var schema = await contentTypeRepository.FindByNameAsync(typeName, ct);

		return schema?.ToSnapshot();
	}
}