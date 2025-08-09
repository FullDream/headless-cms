using Application.ContentTypes.Dtos;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Queries.GetAll;

public record GetContentTypesQuery(ContentTypeKind? Kind) : IRequest<IEnumerable<ContentTypeDto>>;