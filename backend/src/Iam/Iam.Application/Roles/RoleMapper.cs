using Iam.Domain.Roles;

namespace Iam.Application.Roles;

internal static class RoleMapper
{
	internal static RoleDto ToDto(this Role role) =>
		new(role.Id, role.Name, role.Kind, role.Permissions.Select(permission => permission.ToDto()).ToArray());

	private static RolePermissionDto ToDto(this Permission permission) =>
		new(permission.ResourceId, permission.Action, permission.Scope);
}
