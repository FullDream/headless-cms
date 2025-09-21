using ContentTypes.Core;

namespace ContentTypes.Application.Dtos;

public record ContentTypeDto(Guid Id, string Name, ContentTypeKind Kind, IReadOnlyCollection<ContentFieldDto> Fields);