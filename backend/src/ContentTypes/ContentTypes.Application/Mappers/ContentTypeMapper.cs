using ContentTypes.Application.Dtos;
using ContentTypes.Core;
using Contracts;

namespace ContentTypes.Application.Mappers;

public static class ContentTypeMapper
{
	public static ContentTypeDto ToDto(this ContentType contentType) =>
		new ContentTypeDto(contentType.Id,
			contentType.Name,
			contentType.Kind,
			contentType.Fields.Select(f => f.ToDto()).ToArray());

	internal static ContentFieldsSnapshot ToSnapshot(this ContentType ct)
	{
		var dict = ct.Fields.ToDictionary(
			f => f.Name,
			f => new ContentFieldDef(f.Name, f.Type, f.IsRequired, f.Order),
			StringComparer.OrdinalIgnoreCase);

		return new ContentFieldsSnapshot(ct.Id, ct.Name, dict);
	}
}