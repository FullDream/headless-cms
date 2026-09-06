using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;
using ContentTypes.Application.AddField;
using ContentTypes.Application.Authorization;
using ContentTypes.Application.Common.ContentType;
using ContentTypes.Core;

namespace ContentTypes.Application.Create;

public sealed record CreateContentTypeCommand(
	string Name,
	ContentTypeKind Kind,
	IReadOnlyCollection<CreateContentFieldDto> Fields) : ICommand<ContentTypeDto>, IRequirePermission
{
	public PermissionRequirement Permission => ContentTypePermissions.Create;
}
