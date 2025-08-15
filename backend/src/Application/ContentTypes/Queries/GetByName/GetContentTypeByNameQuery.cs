using Application.Common.Messaging;
using Application.ContentTypes.Dtos;

namespace Application.ContentTypes.Queries.GetByName;

public record GetContentTypeByNameQuery(string Name) : IQuery<ContentTypeDto>;