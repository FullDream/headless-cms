using SharedKernel;

namespace ContentTypes.Application.Dtos;

public record ContentFieldDto(Guid Id, string Name, string Label, FieldType Type, bool IsRequired = false);