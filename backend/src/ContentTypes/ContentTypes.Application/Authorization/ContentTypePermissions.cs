using BuildingBlocks.Authorization;

namespace ContentTypes.Application.Authorization;

public static class ContentTypePermissions
{
	public static readonly PermissionRequirement Read = new("content-types", "read");

	public static readonly PermissionRequirement Create = new("content-types", "create");

	public static readonly PermissionRequirement Update = new("content-types", "update");

	public static readonly PermissionRequirement Delete = new("content-types", "delete");
}
