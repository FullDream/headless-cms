using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;
using ContentTypes.Application.Authorization;
using ContentTypes.Application.Common.ContentType;
using ContentTypes.Core;

namespace ContentTypes.Application.GetAll;

public sealed record GetContentTypesQuery(ContentTypeKind? Kind)
	: IQuery<IEnumerable<ContentTypeDto>>, IRequirePermission
{
	public PermissionRequirement Permission => ContentTypePermissions.Read;
}
