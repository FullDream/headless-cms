using SharedKernel;

namespace ContentTypes.Core;

public sealed record ContentFieldDefinition(string Name, string Label, FieldType Type, bool IsRequired);