using Application.ContentTypes.Dtos;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.UpdateContentType;

public record UpdateContentTypeCommand(Guid Id, string? Name, ContentTypeKind? Kind) : IRequest<ContentTypeDto>;