using Application.Common.Messaging;
using Application.ContentTypes.Dtos;
using Core.ContentTypes;

namespace Application.ContentTypes.Commands.Update;

public record UpdateContentTypeCommand(Guid Id, string? Name, ContentTypeKind? Kind) : ICommand<ContentTypeDto>;