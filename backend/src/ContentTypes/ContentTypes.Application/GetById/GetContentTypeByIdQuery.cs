using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;
using ContentTypes.Application.Authorization;
using ContentTypes.Application.Common.ContentType;

namespace ContentTypes.Application.GetById;

public sealed record GetContentTypeByIdQuery(Guid Id) : IQuery<ContentTypeDto>, IRequirePermission
{
	public PermissionRequirement Permission => ContentTypePermissions.Read;
}
