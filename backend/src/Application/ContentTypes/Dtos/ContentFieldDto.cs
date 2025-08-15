using Core.ContentTypes;

namespace Application.ContentTypes.Dtos;

public record ContentFieldDto(Guid Id, string Name, string Label, FieldType Type, bool IsRequired = false);