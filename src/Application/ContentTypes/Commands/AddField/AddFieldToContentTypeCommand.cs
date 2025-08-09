using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Commands.AddField;

public record AddFieldToContentTypeCommand(Guid ContentTypeId, CreateContentFieldDto Field) : IRequest<ContentFieldDto>;