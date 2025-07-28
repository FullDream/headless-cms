namespace Core.ContentTypes;

public class ContentType(Guid id, string name, ContentTypeKind kind)
{
	private readonly List<ContentField> fields = [];
	public Guid Id { get; private set; } = id;
	public string Name { get; private set; } = name;
	public ContentTypeKind Kind { get; private set; } = kind;

	public IReadOnlyCollection<ContentField> Fields => fields.AsReadOnly();

	public void Rename(string name) => Name = name;

	public void ChangeKind(ContentTypeKind kind) => Kind = kind;

	public ContentField AddField(string name, string label, FieldType type, bool isRequired = false)
	{
		ContentField field = new(Guid.NewGuid(), Id, name, label, type, isRequired);

		fields.Add(field);

		return field;
	}

	public IReadOnlyCollection<ContentField> AddFields(
		IEnumerable<(string Name, string Label, FieldType Type, bool IsRequired)> definitions)
	{
		return definitions.Select(d => AddField(d.Name, d.Label, d.Type, d.IsRequired)).ToList();
	}

	public ContentField? RemoveField(Guid fieldId)
	{
		var field = fields.FirstOrDefault(f => f.Id == fieldId);

		if (field is not null)
			fields.Remove(field);

		return field;
	}
}