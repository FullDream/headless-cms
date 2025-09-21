using Application.Abstractions.Messaging;
using ContentTypes.Application.Dtos;

namespace ContentTypes.Application.Commands.RemoveField;

public record RemoveFieldFromContentTypeCommand(Guid ContentTypeId, Guid ContentFieldId) : ICommand<ContentFieldDto>;