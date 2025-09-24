using BuildingBlocks.Messaging;
using ContentTypes.Application.Dtos;

namespace ContentTypes.Application.Commands.Remove;

public record RemoveContentTypeCommand(Guid Id) : ICommand<ContentTypeDto>;