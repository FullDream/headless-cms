using System.Security.Claims;
using BuildingBlocks.Authorization;
using Iam.Domain;
using ClaimTypes = Iam.Infrastructure.Authentication.ClaimTypes;

namespace Iam.Infrastructure.Authorization;

internal static class PermissionClaim
{
	public static string Create(string resource, PermissionAction action, AccessScope scope) =>
		$"{resource}:{action.ToString().ToLowerInvariant()}:{scope.ToString().ToLowerInvariant()}";

	public static bool TryGetScope(Claim claim, string resource, string action, out PermissionAccess access)
	{
		access = PermissionAccess.None;

		if (claim.Type != ClaimTypes.Permission)
			return false;

		var prefix = $"{resource}:{action}:";

		if (!claim.Value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
			return false;

		var scope = claim.Value[prefix.Length..];

		access = scope switch
		{
			"all" => PermissionAccess.All,
			"own" => PermissionAccess.Own,
			_ => PermissionAccess.None
		};

		return access != PermissionAccess.None;
	}
}
