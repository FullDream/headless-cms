using Core.ContentTypes;

namespace Application.ContentTypes.Dtos;

public record UpdateContentFieldDto
{
	public string? Name { get; init; }
	public string? Label { get; init; }
	public FieldType? Type { get; init; }
	public bool? IsRequired { get; init; }
}