using System.Collections.ObjectModel;
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

	public static ContentType Create(string name, ContentTypeKind kind)
	{
		ContentType contentType = new(name, kind);

		contentType.AddDomainEvent(new ContentTypeCreatedEvent(contentType));

		return contentType;
	}

	public void Rename(string name)
	{
		if (Name == name) return;

		Name = name;

		AddDomainEvent(new ContentTypeRenamedEvent(this));
	}

	public void ChangeKind(ContentTypeKind kind)
	{
		if (Kind == kind) return;

		Kind = kind;

		AddDomainEvent(new ContentTypeKindChangedEvent(this));
	}

	public void Remove() => AddDomainEvent(new ContentTypeRemovedEvent(this));

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
		params IReadOnlyCollection<(string Name, string Label, FieldType Type, bool IsRequired)> definitions)
	{
		var duplicate = definitions
			.IntersectBy(Fields.Select(f => f.Name), d => d.Name)
			.FirstOrDefault();

		if (duplicate != default)
			return ContentTypeErrors.ContentFieldNameIsUnique(duplicate.Name);

		var newFields = definitions
			.Select(d => new ContentField(Id, d.Name, d.Label, d.Type, d.IsRequired))
			.ToList();


		if (newFields.Count == 0) return ReadOnlyCollection<ContentField>.Empty;

		fields.AddRange(newFields);

		AddDomainEvent(new ContentFieldsAddedEvent(this));

		return newFields.AsReadOnly();
	}

	public Result<ContentField> RemoveField(Guid fieldId)
	{
		var field = fields.FirstOrDefault(f => f.Id == fieldId);

		if (field is null) return ContentTypeErrors.ContentFieldNotFound(fieldId);

		fields.Remove(field);

		AddDomainEvent(new ContentFieldsRemovedEvent(this));

		return field;
	}
}