using System.Text.Json;
using Application.Abstractions;
using Core.ContentTypes;

namespace Application.ContentEntries.Conversion;

public static class JsonFieldValueConverter
{
	public static object? Convert(JsonElement jsonElement, ContentFieldDef def)
	{
		if (jsonElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
			throw new JsonException($"Property {def.Name} must be a non-null value");

		return def.Type switch
		{
			FieldType.Boolean => jsonElement.GetBoolean(),
			FieldType.Decimal => jsonElement.GetDecimal(),
			FieldType.ShortText or FieldType.LongText => jsonElement.GetString(),
			FieldType.Integer => jsonElement.GetInt64(),
			_ => throw new JsonException($"Unknown field type")
		};
	}
}