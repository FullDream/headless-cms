using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Queries.GetByName;

public record GetContentTypeByNameQuery(string Name) : IRequest<ContentTypeDto?>;