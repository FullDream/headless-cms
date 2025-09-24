using BuildingBlocks.Messaging;
using ContentTypes.Application.Common.ContentType;
using ContentTypes.Core;

namespace ContentTypes.Application.Create;

public sealed record CreateContentTypeCommand(
	string Name,
	ContentTypeKind Kind,
	IReadOnlyCollection<CreateContentFieldDto> Fields) : ICommand<ContentTypeDto>;