using SharedKernel;

namespace ContentTypes.Core;

public record ContentFieldPatch(string? Name, string? Label, FieldType? Type, bool? IsRequired);