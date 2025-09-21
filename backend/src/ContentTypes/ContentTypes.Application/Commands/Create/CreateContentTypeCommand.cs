using Application.Abstractions.Messaging;
using ContentTypes.Application.Dtos;
using ContentTypes.Core;

namespace ContentTypes.Application.Commands.Create;

public record CreateContentTypeCommand(
	string Name,
	ContentTypeKind Kind,
	IReadOnlyCollection<CreateContentFieldDto> Fields) : ICommand<ContentTypeDto>;