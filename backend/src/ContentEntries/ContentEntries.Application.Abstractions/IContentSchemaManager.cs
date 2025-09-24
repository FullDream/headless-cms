using Contracts;

namespace ContentEntries.Application.Abstractions;

public interface IContentSchemaManager
{
	Task EnsureStructureCreatedAsync(ContentTypeSchemaSnapshot schema, CancellationToken ct = default);

	Task RenameStructureAsync(string oldName, string newName, CancellationToken ct = default);
	Task RemoveStructureAsync(ContentTypeSchemaSnapshot schema, CancellationToken ct = default);

	Task AddFieldToStructureAsync(ContentTypeSchemaSnapshot schema, ContentFieldDef field,
		CancellationToken ct = default);

	Task RemoveFieldFromStructureAsync(ContentTypeSchemaSnapshot schema, ContentFieldDef field,
		CancellationToken ct = default);

	Task RenameFieldInStructureAsync(ContentTypeSchemaSnapshot schema, string oldFieldName, string newFieldName,
		CancellationToken ct = default);

	Task ChangeFieldTypeInStructureAsync(ContentTypeSchemaSnapshot schema, ContentFieldDef field,
		CancellationToken ct = default);
}