namespace BuildingBlocks;

public sealed record ContentFieldsSnapshot(
	Guid ContentTypeId,
	string ContentTypeName,
	IReadOnlyDictionary<string, ContentFieldDef> Fields
);