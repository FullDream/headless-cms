using Application.ContentTypes.Dtos;
using Core.ContentTypes;

namespace Application.ContentTypes.Mappers;

public static class ContentTypeMapper
{
	public static ContentTypeDto ToDto(this ContentType contentType) =>
		new ContentTypeDto(contentType.Id, contentType.Name, contentType.Kind,
			contentType.Fields.Select(f => new ContentFieldDto(f.Id, f.Name, f.Label, f.Type, f.IsRequired)).ToArray());
}