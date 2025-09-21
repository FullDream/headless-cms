using Application.Abstractions;
using ContentTypes.Core;

namespace ContentTypes.Infrastructure.Mappers;

internal static class ContentTypeSnapshotMapper
{
	internal static ContentFieldsSnapshot ToSnapshot(this ContentType ct)
	{
		var dict = ct.Fields.ToDictionary(
			f => f.Name,
			f => new ContentFieldDef(f.Name, f.Type, f.IsRequired, f.Order),
			StringComparer.OrdinalIgnoreCase);

		return new ContentFieldsSnapshot(ct.Id, ct.Name, dict);
	}
}