using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;

namespace Iam.Application.Roles.GetRoleById;

public sealed record GetRoleByIdQuery(Guid Id) : IQuery<RoleDto>, IRequirePermission
{
	public PermissionRequirement Permission => RolePermissionRequirements.Read;
}
