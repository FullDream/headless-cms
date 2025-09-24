using System.Collections.ObjectModel;
using ContentEntries.Core;

namespace ContentEntries.Application.Common;

public static class ContentEntryMapper
{
	public static IReadOnlyDictionary<string, object?> ToDto(this ContentEntry contentEntry)
	{
		var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
		{
			["id"] = contentEntry.Id,
			["updatedAt"] = contentEntry.UpdatedAt,
			["createdAt"] = contentEntry.CreatedAt
		};

		foreach (var (name, value) in contentEntry.FieldValues)
			dict[name] = value;

		return new ReadOnlyDictionary<string, object?>(dict);
	}
}