using Iam.Domain.Roles;
using Iam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Iam.Infrastructure.Roles;

internal sealed class RoleSnapshotProvider(IamDbContext dbContext, HybridCache cache)
{
	private const string CacheKey = "iam:roles";

	public async Task<IReadOnlyDictionary<Guid, RolePermissionSnapshot>> GetAsync(
		CancellationToken cancellationToken = default)
	{
		return await cache.GetOrCreateAsync(
			CacheKey,
			async cancellationToken =>
			{
				var roles = await dbContext.Roles.AsNoTracking()
					.Include(x => x.Permissions)
					.ToArrayAsync(cancellationToken);

				return roles.ToDictionary(
					x => x.Id,
					x => new RolePermissionSnapshot(
						x.Id,
						x.Kind,
						[
							.. x.Permissions.Select(permission => new Permission(
								permission.ResourceId,
								permission.Action,
								permission.Scope))
						]));
			},
			cancellationToken: cancellationToken);
	}

	public ValueTask InvalidateAsync(CancellationToken cancellationToken = default) =>
		cache.RemoveAsync(CacheKey, cancellationToken);
}
