using BuildingBlocks.Messaging;
using ContentTypes.Application.Dtos;
using ContentTypes.Core;

namespace ContentTypes.Application.Commands.Update;

public record UpdateContentTypeCommand(Guid Id, string? Name, ContentTypeKind? Kind) : ICommand<ContentTypeDto>;