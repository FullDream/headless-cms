using Application.ContentTypes.Dtos;
using MediatR;

namespace Application.ContentTypes.Commands.AddFieldToContentType;

public record AddFieldToContentTypeCommand(Guid ContentTypeId, ContentFieldDto Field) : IRequest<ContentFieldDto>;