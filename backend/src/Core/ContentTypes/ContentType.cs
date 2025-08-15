using SharedKernel.Result;

namespace Core.ContentTypes;

public class ContentType(string name, ContentTypeKind kind)
{
	private readonly List<ContentField> fields = [];
	public Guid Id { get; } = Guid.NewGuid();
	public string Name { get; private set; } = name;
	public ContentTypeKind Kind { get; private set; } = kind;

	public IReadOnlyCollection<ContentField> Fields => fields.AsReadOnly();

	public void Rename(string name) => Name = name;

	public void ChangeKind(ContentTypeKind kind) => Kind = kind;

	public Result<ContentField> AddField(string name, string label, FieldType type, bool isRequired = false)
	{
		if (Fields.Any(f => f.Name == name))
			return ContentTypeErrors.ContentFieldNameIsUnique(name);

		ContentField field = new(Id, name, label, type, isRequired);

		fields.Add(field);

		return field;
	}

	public Result<ContentField> UpdateField(Guid fieldId, ContentFieldPatch patch)
	{
		ContentField? currentField = fields.FirstOrDefault(f => f.Id == fieldId);

		if (currentField is null)
			return ContentTypeErrors.ContentFieldNotFound(fieldId);

		if (patch.Name is not null && currentField.Name != patch.Name)
		{
			bool isDuplicate = fields.Any(f => f.Id != currentField.Id && f.Name == patch.Name);

			if (isDuplicate)
				return ContentTypeErrors.ContentFieldNameIsUnique(patch.Name);


			currentField.UpdateName(patch.Name);
		}

		if (patch.Label is not null) currentField.UpdateLabel(patch.Label);

		if (patch.IsRequired is not null) currentField.UpdateRequired(patch.IsRequired.Value);

		if (patch.Type is not null) currentField.UpdateType(patch.Type.Value);

		return currentField;
	}

	public Result<IReadOnlyCollection<ContentField>> AddFields(
		IEnumerable<(string Name, string Label, FieldType Type, bool IsRequired)> definitions)
	{
		var results = definitions
			.Select(d => AddField(d.Name, d.Label, d.Type, d.IsRequired))
			.ToList();

		var errors = results
			.Where(r => r.IsFailure)
			.SelectMany(r => r.Errors!)
			.ToArray();

		if (errors.Length != 0)
			return errors;

		return results
			.Select(r => r.Value!)
			.ToList()
			.AsReadOnly();
	}

	public Result<ContentField> RemoveField(Guid fieldId)
	{
		var field = fields.FirstOrDefault(f => f.Id == fieldId);

		if (field is null) return ContentTypeErrors.ContentFieldNotFound(fieldId);

		fields.Remove(field);

		return field;
	}
}