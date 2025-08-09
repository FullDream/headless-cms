using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Queries.GetById;

public record GetContentTypeByIdQuery(Guid Id) : IRequest<ContentTypeDto?>;