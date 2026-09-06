using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;
using ContentTypes.Application.Authorization;
using ContentTypes.Application.Common.ContentType;

namespace ContentTypes.Application.GetByName;

public sealed record GetContentTypeByNameQuery(string Name) : IQuery<ContentTypeDto>, IRequirePermission
{
	public PermissionRequirement Permission => ContentTypePermissions.Read;
}
