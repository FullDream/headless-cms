using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Queries.GetContentTypeByName;

public record GetContentTypeByNameQuery(string Name) : IRequest<ContentTypeDto?>;