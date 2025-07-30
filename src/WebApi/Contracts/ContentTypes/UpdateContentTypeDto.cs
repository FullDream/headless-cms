using Core.ContentTypes;

namespace WebApi.Contracts.ContentTypes;

public record UpdateContentTypeDto(string? Name, ContentTypeKind? Kind);