using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Commands.RemoveContentType;

public record RemoveContentTypeCommand(Guid Id) : IRequest<ContentTypeDto?>;