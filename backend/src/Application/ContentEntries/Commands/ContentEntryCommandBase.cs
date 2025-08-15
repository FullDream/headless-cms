using System.Text.Json;

namespace Application.ContentEntries.Commands;

public abstract record ContentEntryCommandBase(
	string ContentTypeName,
	Dictionary<string, JsonElement> Fields)
{
	private readonly Dictionary<string, object?> converted = new(StringComparer.OrdinalIgnoreCase);

	internal IReadOnlyDictionary<string, object?> ConvertedFields => converted;

	internal void SetConvertedFields(IReadOnlyDictionary<string, object?> values)
	{
		converted.Clear();

		foreach (var (name, value) in values)
			converted[name] = value;
	}
}