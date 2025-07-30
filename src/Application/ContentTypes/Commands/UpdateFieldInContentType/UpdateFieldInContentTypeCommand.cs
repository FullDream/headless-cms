using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Commands.UpdateFieldInContentType;

public record UpdateFieldInContentTypeCommand(Guid ContentTypeId, Guid ContentFieldId, UpdateContentFieldDto UpdateDto)
	: IRequest<ContentFieldDto>;