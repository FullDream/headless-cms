using BuildingBlocks.Authorization;
using BuildingBlocks.Messaging;

namespace Iam.Application.Roles.RemoveRole;

public sealed record RemoveRoleCommand(Guid Id) : ICommand<RoleDto>, IRequirePermission
{
	public PermissionRequirement Permission => RolePermissionRequirements.Delete;
}
