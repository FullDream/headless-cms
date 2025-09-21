using Application.Abstractions.Messaging;
using ContentTypes.Application.Dtos;

namespace ContentTypes.Application.Commands.AddField;

public record AddFieldToContentTypeCommand(Guid ContentTypeId, CreateContentFieldDto Field) : ICommand<ContentFieldDto>;