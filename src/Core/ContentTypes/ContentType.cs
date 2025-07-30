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
		ContentField field = new(Id, name, label, type, isRequired);

		fields.Add(field);

		return field;
	}

	public ContentField UpdateField(Guid fieldId, ContentFieldPatch patch)
	{
		ContentField? currentField = fields.FirstOrDefault(f => f.Id == fieldId) ??
		                             throw new InvalidOperationException($"Field with id {fieldId} not found");

		if (patch.Name is not null && currentField.Name != patch.Name)
		{
			bool isDuplicate = fields.Any(f => f.Id != currentField.Id && f.Name == patch.Name);

			if (isDuplicate)
				throw new InvalidOperationException($"Field name '{patch.Name}' must be unique.");


			currentField.UpdateName(patch.Name);
		}

		if (patch.Label is not null) currentField.UpdateLabel(patch.Label);

		if (patch.IsRequired is not null) currentField.UpdateRequired(patch.IsRequired.Value);

		if (patch.Type is not null) currentField.UpdateType(patch.Type.Value);

		return currentField;
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