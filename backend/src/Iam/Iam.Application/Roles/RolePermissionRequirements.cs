using BuildingBlocks.Authorization;

namespace Iam.Application.Roles;

internal static class RolePermissionRequirements
{
	public static readonly PermissionRequirement Read = new("roles", "read");
	public static readonly PermissionRequirement Create = new("roles", "create");
	public static readonly PermissionRequirement Update = new("roles", "update");
	public static readonly PermissionRequirement Delete = new("roles", "delete");
}
