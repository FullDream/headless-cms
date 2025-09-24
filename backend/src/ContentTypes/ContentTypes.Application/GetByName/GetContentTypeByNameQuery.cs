using BuildingBlocks.Messaging;
using ContentTypes.Application.Common.ContentType;

namespace ContentTypes.Application.GetByName;

public sealed record GetContentTypeByNameQuery(string Name) : IQuery<ContentTypeDto>;