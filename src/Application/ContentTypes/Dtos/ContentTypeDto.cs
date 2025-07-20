using Core.ContentTypes;

namespace Application.ContentTypes.Dtos;

public record ContentTypeDto(Guid Id, string Name, ContentTypeKind Kind);