using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Queries.GetContentTypeById;

public record GetContentTypeByIdQuery(Guid Id) : IRequest<ContentTypeDto?>;