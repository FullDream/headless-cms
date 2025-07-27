using Application.ContentTypes.Dtos;
using Core.ContentTypes;

namespace Application.ContentTypes.Mappers;

public static class ContentFieldMapper
{
	public static ContentFieldDto ToDto(this ContentField contentField) => new ContentFieldDto(contentField.Id,
		contentField.Name, contentField.Label, contentField.Type, contentField.IsRequired);
}