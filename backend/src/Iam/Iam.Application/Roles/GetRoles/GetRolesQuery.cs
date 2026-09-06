using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;

namespace Iam.Application.Roles.GetRoles;

public sealed record GetRolesQuery : IQuery<IReadOnlyCollection<RoleDto>>, IRequirePermission
{
	public PermissionRequirement Permission => RolePermissionRequirements.Read;
}
