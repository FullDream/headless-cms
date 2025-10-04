using Contracts;

namespace ContentTypes.Application.Common.ContentField;

internal static class ContentFieldMapper
{
	public static ContentFieldDto ToDto(this Core.ContentField contentField) => new ContentFieldDto(contentField.Id,
		contentField.Name,
		contentField.Label,
		contentField.Type,
		contentField.IsRequired);

	public static ContentFieldDef ToDef(this Core.ContentField contentField) => new ContentFieldDef(
		contentField.Name,
		contentField.Type,
		contentField.IsRequired);
}