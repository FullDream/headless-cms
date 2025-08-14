using Application.Common.Messaging;
using Application.ContentTypes.Dtos;
using Core.ContentTypes;

namespace Application.ContentTypes.Commands.Create;

public record CreateContentTypeCommand(
	string Name,
	ContentTypeKind Kind,
	IReadOnlyCollection<CreateContentFieldDto> Fields) : ICommand<ContentTypeDto>;