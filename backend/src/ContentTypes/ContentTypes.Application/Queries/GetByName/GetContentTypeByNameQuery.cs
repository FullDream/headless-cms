using BuildingBlocks.Messaging;
using ContentTypes.Application.Dtos;

namespace ContentTypes.Application.Queries.GetByName;

public record GetContentTypeByNameQuery(string Name) : IQuery<ContentTypeDto>;