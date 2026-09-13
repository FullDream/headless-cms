using BuildingBlocks.Authorization;

namespace Iam.Application.Users;

internal static class UserPermissionRequirements
{
	public static readonly PermissionRequirement Read = new("users", "read");
	public static readonly PermissionRequirement Update = new("users", "update");
}
