using Core.ContentTypes;

namespace WebApi.Contracts.ContentTypes;

public record UpdateContentTypeDto
{
	public string? Name { get; init; }
	public ContentTypeKind? Kind { get; init; }
};