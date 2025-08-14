using Application.Common.Messaging;
using Application.ContentTypes.Dtos;

namespace Application.ContentTypes.Queries.GetById;

public record GetContentTypeByIdQuery(Guid Id) : IQuery<ContentTypeDto>;