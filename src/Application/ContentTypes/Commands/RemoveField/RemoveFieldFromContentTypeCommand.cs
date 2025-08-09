using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Commands.RemoveField;

public record RemoveFieldFromContentTypeCommand(Guid ContentTypeId, Guid ContentFieldId) : IRequest<ContentFieldDto?>;