using Application.Common.Messaging;
using Application.ContentTypes.Dtos;

namespace Application.ContentTypes.Commands.Remove;

public record RemoveContentTypeCommand(Guid Id) : ICommand<ContentTypeDto>;