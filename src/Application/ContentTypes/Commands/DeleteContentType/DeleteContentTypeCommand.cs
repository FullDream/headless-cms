using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Commands.DeleteContentType;

public record DeleteContentTypeCommand(Guid Id) : IRequest<ContentTypeDto?>;