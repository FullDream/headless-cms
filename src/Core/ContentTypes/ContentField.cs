namespace Core.ContentTypes;

public class ContentField(string name, string label, FieldType type, bool isRequired = false)
{
	public string Name { get; } = name;
	public string Label { get; } = label;
	public FieldType Type { get; } = type;
	public bool IsRequired { get; } = isRequired;
}