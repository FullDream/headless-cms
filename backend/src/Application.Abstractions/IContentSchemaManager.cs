namespace Application.Abstractions;

public interface IContentSchemaManager
{
	Task EnsureStructureCreatedAsync(ContentFieldsSnapshot schema, CancellationToken ct = default);

	Task RenameStructureAsync(string oldName, string newName, CancellationToken ct = default);
	Task RemoveStructureAsync(ContentFieldsSnapshot schema, CancellationToken ct = default);

	Task AddFieldToStructureAsync(ContentFieldsSnapshot schema, ContentFieldDef field, CancellationToken ct = default);

	Task RemoveFieldFromStructureAsync(ContentFieldsSnapshot schema, ContentFieldDef field,
		CancellationToken ct = default);

	Task RenameFieldInStructureAsync(ContentFieldsSnapshot schema, string oldFieldName, string newFieldName,
		CancellationToken ct = default);

	Task ChangeFieldTypeInStructureAsync(ContentFieldsSnapshot schema, ContentFieldDef field,
		CancellationToken ct = default);
}