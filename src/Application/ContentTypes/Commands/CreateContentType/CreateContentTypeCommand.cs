using Application.ContentTypes.Dtos;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.CreateContentType;

public record CreateContentTypeCommand(string Name, ContentTypeKind Kind) : IRequest<ContentTypeDto>;