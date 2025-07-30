namespace Core.ContentTypes;

public class ContentField
{
	internal ContentField(Guid contentTypeId, string name, string label, FieldType type,
		bool isRequired = false)
	{
		ContentTypeId = contentTypeId;
		Name = name;
		Label = label;
		Type = type;
		IsRequired = isRequired;
	}

	public Guid Id { get; private set; } = Guid.NewGuid();
	public Guid ContentTypeId { get; private set; }
	public string Name { get; private set; }
	public string Label { get; private set; }
	public FieldType Type { get; private set; }
	public bool IsRequired { get; private set; }
	public int Order { get; private set; } = 0;

	internal void UpdateName(string name)
	{
		Name = name;
	}

	internal void UpdateLabel(string label)
	{
		Label = label;
	}

	internal void UpdateType(FieldType type)
	{
		Type = type;
	}

	internal void UpdateRequired(bool isRequired)
	{
		IsRequired = isRequired;
	}

	internal void UpdateOrder(int order)
	{
		Order = order;
	}
}