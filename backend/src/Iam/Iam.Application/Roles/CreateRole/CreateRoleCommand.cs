using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;

namespace Iam.Application.Roles.CreateRole;

public sealed record CreateRoleCommand(string Name, IReadOnlyCollection<RolePermissionDto> Permissions)
	: ICommand<RoleDto>, IRequirePermission
{
	public PermissionRequirement Permission => RolePermissionRequirements.Create;
}
