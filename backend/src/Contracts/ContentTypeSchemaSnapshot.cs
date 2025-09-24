namespace Contracts;

public sealed record ContentTypeSchemaSnapshot(
	Guid ContentTypeId,
	string ContentTypeName,
	IReadOnlyDictionary<string, ContentFieldDef> Fields
);