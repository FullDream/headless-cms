using Application.Abstractions.Messaging;
using ContentTypes.Application.Dtos;

namespace ContentTypes.Application.Queries.GetById;

public record GetContentTypeByIdQuery(Guid Id) : IQuery<ContentTypeDto>;