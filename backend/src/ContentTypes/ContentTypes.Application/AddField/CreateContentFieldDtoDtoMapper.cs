using ContentTypes.Core;

namespace ContentTypes.Application.AddField;

internal static class CreateContentFieldDtoDtoMapper
{
	public static ContentFieldDefinition ToDefinition(this CreateContentFieldDto dto) => new ContentFieldDefinition(
		dto.Name,
		dto.Label,
		dto.Type,
		dto.IsRequired);
}