using ContentTypes.Application.Common.ContentField;
using Contracts;

namespace ContentTypes.Application.Common.ContentType;

internal static class ContentTypeMapper
{
	public static ContentTypeDto ToDto(this Core.ContentType contentType) =>
		new ContentTypeDto(contentType.Id,
			contentType.Name,
			contentType.Kind,
			contentType.Fields.Select(f => f.ToDto()).ToArray());

	public static ContentFieldsSnapshot ToSnapshot(this Core.ContentType ct)
	{
		var dict = ct.Fields.ToDictionary(
			f => f.Name,
			f => new ContentFieldDef(f.Name, f.Type, f.IsRequired, f.Order),
			StringComparer.OrdinalIgnoreCase);

		return new ContentFieldsSnapshot(ct.Id, ct.Name, dict);
	}
}