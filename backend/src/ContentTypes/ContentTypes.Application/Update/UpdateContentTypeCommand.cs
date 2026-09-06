using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;
using ContentTypes.Application.Authorization;
using ContentTypes.Application.Common.ContentType;
using ContentTypes.Core;

namespace ContentTypes.Application.Update;

public sealed record UpdateContentTypeCommand(Guid Id, string? Name, ContentTypeKind? Kind)
	: ICommand<ContentTypeDto>, IRequirePermission
{
	public PermissionRequirement Permission => ContentTypePermissions.Update;
}
