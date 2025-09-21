using Application.Abstractions.Messaging;
using ContentTypes.Application.Dtos;
using ContentTypes.Core;

namespace ContentTypes.Application.Queries.GetAll;

public record GetContentTypesQuery(ContentTypeKind? Kind) : IQuery<IEnumerable<ContentTypeDto>>;