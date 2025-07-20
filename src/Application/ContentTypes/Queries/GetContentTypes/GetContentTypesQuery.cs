using Application.ContentTypes.Dtos;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Queries.GetContentTypes;

public record GetContentTypesQuery(ContentTypeKind? Kind) : IRequest<IEnumerable<ContentTypeDto>>;