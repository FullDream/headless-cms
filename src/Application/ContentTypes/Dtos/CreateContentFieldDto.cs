using Core.ContentTypes;

namespace Application.ContentTypes.Dtos;

public record CreateContentFieldDto(string Name, string Label, FieldType Type, bool IsRequired = false);