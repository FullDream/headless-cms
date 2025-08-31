using Application.ContentTypes.Dtos;
using Core.ContentTypes;

namespace WebApi.Contracts.ContentTypes;

public record CreateContentTypeDto(string Name, ContentTypeKind Kind)
{
	public IReadOnlyCollection<CreateContentFieldDto> Fields { get; init; } = [];
}