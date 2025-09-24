using SharedKernel;

namespace ContentTypes.Application.Create;

public sealed record CreateContentFieldDto(string Name, string Label, FieldType Type, bool IsRequired = false);