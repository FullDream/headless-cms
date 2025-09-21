using SharedKernel;

namespace ContentTypes.Application.Dtos;

public record CreateContentFieldDto(string Name, string Label, FieldType Type, bool IsRequired = false);