using Application.Abstractions.Messaging;
using ContentTypes.Application.Dtos;

namespace ContentTypes.Application.Commands.UpdateField;

public record UpdateFieldInContentTypeCommand(Guid ContentTypeId, Guid ContentFieldId, UpdateContentFieldDto UpdateDto)
	: ICommand<ContentFieldDto>;