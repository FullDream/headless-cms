namespace Core.ContentTypes;

public class ContentField
{
	internal ContentField(Guid id, Guid contentTypeId, string name, string label, FieldType type,
		bool isRequired = false)
	{
		Id = id;
		ContentTypeId = contentTypeId;
		Name = name;
		Label = label;
		Type = type;
		IsRequired = isRequired;
	}

	public Guid Id { get; private set; }
	public Guid ContentTypeId { get; private set; }
	public string Name { get; private set; }
	public string Label { get; private set; }
	public FieldType Type { get; private set; }
	public bool IsRequired { get; private set; }
	public int Order { get; private set; } = 0;
}