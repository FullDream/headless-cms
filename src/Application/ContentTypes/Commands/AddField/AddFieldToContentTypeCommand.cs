using Application.Common.Messaging;
using Application.ContentTypes.Dtos;

namespace Application.ContentTypes.Commands.AddField;

public record AddFieldToContentTypeCommand(Guid ContentTypeId, CreateContentFieldDto Field) : ICommand<ContentFieldDto>;