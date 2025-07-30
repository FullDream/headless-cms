namespace Core.ContentTypes;

public record ContentFieldPatch(string? Name, string? Label, FieldType? Type, bool? IsRequired);