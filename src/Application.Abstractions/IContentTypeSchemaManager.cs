using Core.ContentTypes;

namespace Application.Abstractions;

public interface IContentTypeSchemaManager
{
	Task EnsureStructureCreatedAsync(ContentType contentType, CancellationToken ct = default);

	Task RenameStructureAsync(string oldName, string newName, CancellationToken ct = default);
	Task RemoveStructureAsync(ContentType contentType, CancellationToken ct = default);

	Task AddFieldToStructureAsync(ContentType contentType, ContentField field, CancellationToken ct = default);
	Task RemoveFieldFromStructureAsync(ContentType contentType, ContentField field, CancellationToken ct = default);

	Task RenameFieldInStructureAsync(ContentType contentType, string oldFieldName, string newFieldName,
		CancellationToken ct = default);

	Task ChangeFieldTypeInStructureAsync(ContentType contentType, ContentField field, CancellationToken ct = default);
}