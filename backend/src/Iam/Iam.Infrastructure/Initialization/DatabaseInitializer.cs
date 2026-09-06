using Iam.Domain;
using Iam.Domain.AccessResource;
using Iam.Domain.Roles;
using Iam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Iam.Infrastructure.Initialization;

internal sealed class DatabaseInitializer(IamDbContext dbContext, IRoleRepository roleRepository)
{
	public async Task InitializeAsync(CancellationToken cancellationToken = default)
	{
		await EnsureSystemResourcesAsync(cancellationToken);
		await EnsureSuperAdminRoleAsync(cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	private async Task EnsureSuperAdminRoleAsync(CancellationToken cancellationToken)
	{
		var role = await roleRepository.FindByIdAsync(SystemRoleIds.SuperAdmin, cancellationToken);

		if (role is null)
		{
			await roleRepository.AddAsync(Role.CreateSuperAdmin(SystemRoleIds.SuperAdmin), cancellationToken);

			return;
		}

		if (role.Kind != RoleKind.SuperAdmin)
			throw new InvalidOperationException(
				$"System role '{SystemRoleIds.SuperAdmin}' has invalid kind '{role.Kind}'.");
	}


	private async Task EnsureSystemResourcesAsync(CancellationToken cancellationToken)
	{
		var existingIds = await dbContext.Resources.Select(x => x.Id).ToHashSetAsync(cancellationToken);

		foreach (var resource in CreateSystemResources())
		{
			if (!existingIds.Contains(resource.Id))
				dbContext.Resources.Add(resource);
		}
	}

	private static IEnumerable<AccessResource> CreateSystemResources()
	{
		yield return Resource(
			SystemAccessResourceIds.ContentTypes,
			"content-types",
			"Content Types",
			[
				Capability(PermissionAction.Read, AccessScope.All),
				Capability(PermissionAction.Create, AccessScope.All),
				Capability(PermissionAction.Update, AccessScope.All),
				Capability(PermissionAction.Delete, AccessScope.All)
			]);

		yield return Resource(SystemAccessResourceIds.ContentEntries, "content-entries", "Content Entries", []);

		yield return Resource(
			SystemAccessResourceIds.Roles,
			"roles",
			"Roles",
			[
				Capability(PermissionAction.Read, AccessScope.All),
				Capability(PermissionAction.Create, AccessScope.All),
				Capability(PermissionAction.Update, AccessScope.All),
				Capability(PermissionAction.Delete, AccessScope.All)
			]);
	}

	private static AccessResource Resource(
		Guid id,
		string key,
		string name,
		IEnumerable<ResourceCapability> capabilities)
	{
		var result = AccessResource.Create(id, key, name, capabilities);

		if (result.IsFailure)
			throw new InvalidOperationException($"Invalid system permission resource '{key}'.");

		return result.Value;
	}

	private static ResourceCapability Capability(PermissionAction action, params AccessScope[] scopes)
	{
		var result = ResourceCapability.Create(action, scopes);

		if (result.IsFailure)
			throw new InvalidOperationException($"Invalid system capability: {action}");

		return result.Value;
	}
}
