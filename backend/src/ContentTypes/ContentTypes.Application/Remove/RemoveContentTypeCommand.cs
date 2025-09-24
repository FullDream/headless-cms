using BuildingBlocks.Messaging;
using ContentTypes.Application.Common.ContentType;

namespace ContentTypes.Application.Remove;

public sealed record RemoveContentTypeCommand(Guid Id) : ICommand<ContentTypeDto>;