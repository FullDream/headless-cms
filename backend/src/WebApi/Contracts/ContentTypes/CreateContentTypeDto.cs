using ContentTypes.Application.Dtos;
using ContentTypes.Core;

namespace WebApi.Contracts.ContentTypes;

public record CreateContentTypeDto(string Name, ContentTypeKind Kind)
{
	public IReadOnlyCollection<CreateContentFieldDto> Fields { get; init; } = [];
}