using Core.ContentTypes;

namespace Application.ContentTypes.Dtos;

public record UpdateContentFieldDto(string? Name, string? Label, FieldType? Type, bool? IsRequired);