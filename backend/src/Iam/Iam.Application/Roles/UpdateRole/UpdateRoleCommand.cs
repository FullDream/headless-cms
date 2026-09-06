using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;

namespace Iam.Application.Roles.UpdateRole;

public sealed record UpdateRoleCommand(Guid Id, string Name, IReadOnlyCollection<RolePermissionDto> Permissions)
	: ICommand<RoleDto>, IRequirePermission
{
	public PermissionRequirement Permission => RolePermissionRequirements.Update;
}
