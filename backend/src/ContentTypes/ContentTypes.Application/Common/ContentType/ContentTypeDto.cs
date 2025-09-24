using ContentTypes.Application.Common.ContentField;
using ContentTypes.Core;

namespace ContentTypes.Application.Common.ContentType;

public record ContentTypeDto(Guid Id, string Name, ContentTypeKind Kind, IReadOnlyCollection<ContentFieldDto> Fields);