using Application.Common.Messaging;
using Application.ContentTypes.Dtos;

namespace Application.ContentTypes.Commands.RemoveField;

public record RemoveFieldFromContentTypeCommand(Guid ContentTypeId, Guid ContentFieldId) : ICommand<ContentFieldDto>;