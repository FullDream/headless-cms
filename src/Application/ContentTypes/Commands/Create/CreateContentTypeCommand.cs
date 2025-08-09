using Application.ContentTypes.Dtos;
using Core.ContentTypes;
using MediatR;

namespace Application.ContentTypes.Commands.Create;

public record CreateContentTypeCommand(
	string Name,
	ContentTypeKind Kind,
	IReadOnlyCollection<CreateContentFieldDto> Fields) : IRequest<ContentTypeDto>;