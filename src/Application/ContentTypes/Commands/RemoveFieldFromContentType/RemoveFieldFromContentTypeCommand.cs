using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Commands.RemoveFieldFromContentType;

public record RemoveFieldFromContentTypeCommand(Guid ContentTypeId, Guid ContentFieldId) : IRequest<ContentFieldDto?>;