using Application.Common.Messaging;
using Application.ContentTypes.Dtos;
using Core.ContentTypes;

namespace Application.ContentTypes.Queries.GetAll;

public record GetContentTypesQuery(ContentTypeKind? Kind) : IQuery<IEnumerable<ContentTypeDto>>;