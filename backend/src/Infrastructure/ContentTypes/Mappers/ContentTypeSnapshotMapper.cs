using Application.Abstractions;
using Core.ContentTypes;

namespace Infrastructure.ContentTypes.Mappers;

internal static class ContentTypeSnapshotMapper
{
	internal static ContentFieldsSnapshot ToSnapshot(this ContentType ct)
	{
		var dict = ct.Fields.ToDictionary(
			f => f.Name,
			f => new ContentFieldDef(f.Name, f.Type, f.IsRequired, f.Order),
			StringComparer.OrdinalIgnoreCase);

		return new ContentFieldsSnapshot(ct.Id, dict);
	}
}