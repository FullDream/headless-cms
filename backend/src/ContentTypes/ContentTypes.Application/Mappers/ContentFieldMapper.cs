using ContentTypes.Application.Dtos;
using ContentTypes.Core;

namespace ContentTypes.Application.Mappers;

public static class ContentFieldMapper
{
	public static ContentFieldDto ToDto(this ContentField contentField) => new ContentFieldDto(contentField.Id,
		contentField.Name,
		contentField.Label,
		contentField.Type,
		contentField.IsRequired);
}