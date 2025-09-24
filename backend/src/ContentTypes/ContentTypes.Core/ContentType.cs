using ContentTypes.Core.Events;
using SharedKernel;
using SharedKernel.Result;

namespace ContentTypes.Core;

public class ContentType() : AggregateRoot
{
	private readonly List<ContentField> fields = [];

	private ContentType(string name, ContentTypeKind kind) : this()
	{
		Name = name;
		Kind = kind;
	}

	public Guid Id { get; } = Guid.NewGuid();
	public string Name { get; private set; }
	public ContentTypeKind Kind { get; private set; }
	public IReadOnlyCollection<ContentField> Fields => fields.AsReadOnly();

	public static Result<ContentType> Create(string name, ContentTypeKind kind,
		IReadOnlyCollection<ContentFieldDefinition> fieldDefinitions)
	{
		ContentType contentType = new(name, kind);

		var duplicate = fieldDefinitions
			.GroupBy(f => f.Name)
			.FirstOrDefault(g => g.Count() > 1);

		if (duplicate is not null)
			return ContentTypeErrors.ContentFieldNameIsUnique(duplicate.Key);

		contentType.fields.AddRange(fieldDefinitions.Select(f => new ContentField(contentType.Id,
			f.Name,
			f.Label,
			f.Type,
			f.IsRequired)));

		contentType.AddDomainEvent(new ContentTypeCreatedEvent(contentType));

		return contentType;
	}

	public void Rename(string name)
	{
		if (Name == name) return;

		var oldName = Name;

		Name = name;

		AddDomainEvent(new ContentTypeRenamedEvent(this, oldName));
	}

	public void ChangeKind(ContentTypeKind kind)
	{
		if (Kind == kind) return;

		Kind = kind;

		AddDomainEvent(new ContentTypeKindChangedEvent(this));
	}

	public void Remove() => AddDomainEvent(new ContentTypeRemovedEvent(this));


	public Result<ContentField> AddField(ContentFieldDefinition fieldDefinition)
	{
		if (fields.Any(f => f.Name == fieldDefinition.Name))
			return ContentTypeErrors.ContentFieldNameIsUnique(fieldDefinition.Name);

		var field = new ContentField(Id,
			fieldDefinition.Name,
			fieldDefinition.Label,
			fieldDefinition.Type,
			fieldDefinition.IsRequired);

		fields.Add(field);

		AddDomainEvent(new ContentFieldAddedEvent(this, field));

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

	public Result<ContentField> RemoveField(Guid fieldId)
	{
		var field = fields.FirstOrDefault(f => f.Id == fieldId);

		if (field is null) return ContentTypeErrors.ContentFieldNotFound(fieldId);

		fields.Remove(field);

		AddDomainEvent(new ContentFieldsRemovedEvent(this, field));

		return field;
	}
}