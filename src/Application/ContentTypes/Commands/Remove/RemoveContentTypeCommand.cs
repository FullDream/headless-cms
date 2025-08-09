using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Commands.Remove;

public record RemoveContentTypeCommand(Guid Id) : IRequest<ContentTypeDto?>;