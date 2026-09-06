using System.Security.Claims;
using Iam.Domain;
using Iam.Domain.Roles;
using Iam.Infrastructure.Persistence;
using Iam.Infrastructure.Roles;
using Iam.Infrastructure.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Iam.Infrastructure.Authentication;

internal sealed class ClaimsPrincipalFactory(
	UserManager<PersistenceUser> userManager,
	RoleManager<PersistenceRole> roleManager,
	IOptions<IdentityOptions> options,
	IamDbContext dbContext)
	: UserClaimsPrincipalFactory<PersistenceUser, PersistenceRole>(userManager, roleManager, options)
{
	protected override async Task<ClaimsIdentity> GenerateClaimsAsync(PersistenceUser user)
	{
		var identity = await base.GenerateClaimsAsync(user);

		var roles = await dbContext.UserRoles.Where(x => x.UserId == user.Id)
			.Join(
				dbContext.Roles,
				userRole => userRole.RoleId,
				role => role.Id,
				(_, role) => new
				{
					role.Id,
					role.Kind
				})
			.ToListAsync();

		if (roles.Any(x => x.Kind == RoleKind.SuperAdmin))
		{
			identity.AddClaim(new Claim(ClaimTypes.SuperAdmin, bool.TrueString));

			return identity;
		}

		var roleIds = roles.Select(x => x.Id).ToArray();

		if (roleIds.Length == 0)
			return identity;

		var permissions = await dbContext.RolePermissions.Where(x => roleIds.Contains(x.RoleId))
			.Join(
				dbContext.Resources,
				permission => permission.ResourceId,
				resource => resource.Id,
				(permission, resource) => new
				{
					resource.Key,
					permission.Action,
					permission.Scope
				})
			.ToListAsync();

		var effectivePermissions = permissions.GroupBy(x => new { x.Key, x.Action })
			.Select(group => new
			{
				group.Key.Key,
				group.Key.Action,
				Scope = group.Any(x => x.Scope == AccessScope.All) ? AccessScope.All : AccessScope.Own
			});

		foreach (var permission in effectivePermissions)
		{
			identity.AddClaim(
				new Claim(
					ClaimTypes.Permission,
					CreatePermissionClaimValue(permission.Key, permission.Action, permission.Scope)));
		}

		return identity;
	}

	private static string CreatePermissionClaimValue(string resource, PermissionAction action, AccessScope scope) =>
		$"{resource}:{action.ToString().ToLowerInvariant()}:{scope.ToString().ToLowerInvariant()}";
}
