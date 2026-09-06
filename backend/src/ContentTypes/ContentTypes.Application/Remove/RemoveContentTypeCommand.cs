using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;
using ContentTypes.Application.Authorization;
using ContentTypes.Application.Common.ContentType;

namespace ContentTypes.Application.Remove;

public sealed record RemoveContentTypeCommand(Guid Id) : ICommand<ContentTypeDto>, IRequirePermission
{
	public PermissionRequirement Permission => ContentTypePermissions.Delete;
}
