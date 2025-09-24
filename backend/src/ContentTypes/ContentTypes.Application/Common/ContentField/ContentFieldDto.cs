using SharedKernel;

namespace ContentTypes.Application.Common.ContentField;

public record ContentFieldDto(Guid Id, string Name, string Label, FieldType Type, bool IsRequired = false);