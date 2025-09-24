using SharedKernel;

namespace ContentTypes.Application.AddField;

public sealed record CreateContentFieldDto(string Name, string Label, FieldType Type, bool IsRequired = false);