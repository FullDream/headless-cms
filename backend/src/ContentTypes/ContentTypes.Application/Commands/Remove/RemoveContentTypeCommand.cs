using Application.Abstractions.Messaging;
using ContentTypes.Application.Dtos;

namespace ContentTypes.Application.Commands.Remove;

public record RemoveContentTypeCommand(Guid Id) : ICommand<ContentTypeDto>;