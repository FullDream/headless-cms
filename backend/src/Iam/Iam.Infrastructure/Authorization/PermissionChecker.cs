using System.Security.Claims;
using BuildingBlocks.Authorization;
using Iam.Domain;
using Iam.Domain.Roles;
using Iam.Infrastructure.Authorization.AccessResources;
using Iam.Infrastructure.Roles;
using Iam.Infrastructure.Users;
using Microsoft.AspNetCore.Http;

namespace Iam.Infrastructure.Authorization;

internal sealed class PermissionChecker(
	IHttpContextAccessor httpContextAccessor,
	UserRoleProvider userRoleProvider,
	RoleSnapshotProvider roleSnapshotProvider,
	AccessResourceProvider accessResourceProvider) : IPermissionChecker
{
	public async Task<PermissionAccess> CheckAsync(
		PermissionRequirement permission,
		CancellationToken cancellationToken = default)
	{
		var principal = httpContextAccessor.HttpContext?.User;

		if (principal?.Identity is not { IsAuthenticated: true })
			return PermissionAccess.None;

		if (!Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
			return PermissionAccess.None;

		var action = ParseAction(permission.Action);

		if (action is null)
			return PermissionAccess.None;

		var resources = await accessResourceProvider.GetAsync(cancellationToken);

		if (!resources.TryGetValue(permission.Resource, out var resource))
			return PermissionAccess.None;

		var roleIds = await userRoleProvider.GetRoleIdsAsync(userId, cancellationToken);
		var roles = await roleSnapshotProvider.GetAsync(cancellationToken);
		var access = PermissionAccess.None;

		foreach (var roleId in roleIds)
		{
			if (!roles.TryGetValue(roleId, out var role))
				continue;

			if (role.Kind == RoleKind.SuperAdmin)
				return PermissionAccess.All;

			foreach (var rolePermission in role.Permissions)
			{
				if (rolePermission.ResourceId != resource.Id || rolePermission.Action != action.Value)
					continue;

				var permissionAccess = ResolveAccess(rolePermission.Scope);

				if (permissionAccess == PermissionAccess.All)
					return PermissionAccess.All;

				if (permissionAccess == PermissionAccess.Own)
					access = PermissionAccess.Own;
			}
		}

		return access;
	}

	private static PermissionAction? ParseAction(string action) =>
		action switch
		{
			"read" => PermissionAction.Read,
			"create" => PermissionAction.Create,
			"update" => PermissionAction.Update,
			"delete" => PermissionAction.Delete,
			"publish" => PermissionAction.Publish,
			_ => null
		};

	private static PermissionAccess ResolveAccess(AccessScope scope) =>
		scope switch
		{
			AccessScope.All => PermissionAccess.All,
			AccessScope.Own => PermissionAccess.Own,
			_ => PermissionAccess.None
		};
}
