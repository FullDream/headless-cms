namespace Application.Abstractions;

public sealed record ContentFieldsSnapshot(
	Guid ContentTypeId,
	IReadOnlyDictionary<string, ContentFieldDef> Fields
);