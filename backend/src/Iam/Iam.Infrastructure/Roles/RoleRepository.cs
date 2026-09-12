using Iam.Domain.Roles;
using Iam.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Iam.Infrastructure.Roles;

internal sealed class RoleRepository(RoleManager<PersistenceRole> roleManager, IamDbContext dbContext) : IRoleRepository
{
	public async Task<IReadOnlyCollection<Role>> FindManyAsync(CancellationToken cancellationToken = default)
	{
		var roles = await dbContext.Roles.AsNoTracking()
			.Include(x => x.Permissions)
			.OrderBy(x => x.Kind == RoleKind.Custom)
			.ThenBy(x => x.Name)
			.ToArrayAsync(cancellationToken);

		return roles.Select(MapToDomain).ToArray();
	}

	public async Task<Role?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var entity = await dbContext.Roles.AsNoTracking()
			.Include(x => x.Permissions)
			.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

		return entity is null ? null : MapToDomain(entity);
	}

	public async Task<Role?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
	{
		var normalizedName = roleManager.NormalizeKey(name);

		var entity = await dbContext.Roles.AsNoTracking()
			.Include(x => x.Permissions)
			.FirstOrDefaultAsync(x => x.NormalizedName == normalizedName, cancellationToken);

		return entity is null ? null : MapToDomain(entity);
	}

	public async Task AddAsync(Role role, CancellationToken cancellationToken = default)
	{
		await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

		var persistenceRole = MapToPersistence(role);

		var result = await roleManager.CreateAsync(persistenceRole);

		if (!result.Succeeded)
			throw CreateIdentityException(result);

		dbContext.RolePermissions.AddRange(role.Permissions.Select(permission => MapPermission(role.Id, permission)));

		await dbContext.SaveChangesAsync(cancellationToken);
		await transaction.CommitAsync(cancellationToken);
	}

	public async Task UpdateAsync(Role role, CancellationToken cancellationToken = default)
	{
		await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

		var entity = await roleManager.FindByIdAsync(role.Id.ToString());

		if (entity is null)
			throw new InvalidOperationException($"Role '{role.Id}' was not found.");

		entity.Name = role.Name;
		entity.Kind = role.Kind;

		var result = await roleManager.UpdateAsync(entity);

		if (!result.Succeeded)
			throw CreateIdentityException(result);

		await dbContext.RolePermissions.Where(x => x.RoleId == role.Id).ExecuteDeleteAsync(cancellationToken);

		dbContext.RolePermissions.AddRange(role.Permissions.Select(permission => MapPermission(role.Id, permission)));

		await dbContext.SaveChangesAsync(cancellationToken);
		await transaction.CommitAsync(cancellationToken);
	}

	public async Task RemoveAsync(Role role, CancellationToken cancellationToken = default)
	{
		var entity = await roleManager.FindByIdAsync(role.Id.ToString());

		if (entity is null)
			return;

		var result = await roleManager.DeleteAsync(entity);

		if (!result.Succeeded)
			throw CreateIdentityException(result);
	}

	private static Role MapToDomain(PersistenceRole entity) =>
		Role.Restore(
			entity.Id,
			entity.Name!,
			entity.Kind,
			entity.Permissions.Select(x => new Permission(x.ResourceId, x.Action, x.Scope)));

	private static PersistenceRole MapToPersistence(Role role) =>
		new()
		{
			Id = role.Id,
			Name = role.Name,
			Kind = role.Kind,
		};

	private static PersistencePermission MapPermission(Guid roleId, Permission permission) =>
		new()
		{
			RoleId = roleId,
			ResourceId = permission.ResourceId,
			Action = permission.Action,
			Scope = permission.Scope
		};

	private static InvalidOperationException CreateIdentityException(IdentityResult result) =>
		new(string.Join("; ", result.Errors.Select(x => x.Description)));
}
