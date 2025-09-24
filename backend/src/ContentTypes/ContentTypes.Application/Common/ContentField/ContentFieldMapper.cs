namespace ContentTypes.Application.Common.ContentField;

internal static class ContentFieldMapper
{
	public static ContentFieldDto ToDto(this Core.ContentField contentField) => new ContentFieldDto(contentField.Id,
		contentField.Name,
		contentField.Label,
		contentField.Type,
		contentField.IsRequired);
}