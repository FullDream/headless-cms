using Application.Common.Messaging;
using Application.ContentTypes.Dtos;

namespace Application.ContentTypes.Commands.UpdateField;

public record UpdateFieldInContentTypeCommand(Guid ContentTypeId, Guid ContentFieldId, UpdateContentFieldDto UpdateDto)
	: ICommand<ContentFieldDto>;