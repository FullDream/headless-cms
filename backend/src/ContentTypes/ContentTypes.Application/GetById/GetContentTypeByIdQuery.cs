using BuildingBlocks.Messaging;
using ContentTypes.Application.Common.ContentType;

namespace ContentTypes.Application.GetById;

public sealed record GetContentTypeByIdQuery(Guid Id) : IQuery<ContentTypeDto>;