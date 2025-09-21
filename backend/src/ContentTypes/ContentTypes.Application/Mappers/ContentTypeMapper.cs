using ContentTypes.Application.Dtos;
using ContentTypes.Core;

namespace ContentTypes.Application.Mappers;

public static class ContentTypeMapper
{
	public static ContentTypeDto ToDto(this ContentType contentType) =>
		new ContentTypeDto(contentType.Id,
			contentType.Name,
			contentType.Kind,
			contentType.Fields.Select(f => f.ToDto()).ToArray());
}