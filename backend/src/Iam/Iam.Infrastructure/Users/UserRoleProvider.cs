using Iam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Iam.Infrastructure.Users;

internal sealed class UserRoleProvider(IamDbContext dbContext, HybridCache cache)
{
	public async Task<IReadOnlyCollection<Guid>> GetRoleIdsAsync(
		Guid userId,
		CancellationToken cancellationToken = default)
	{
		return await cache.GetOrCreateAsync(
			GetCacheKey(userId),
			async cancellationToken => await dbContext.UserRoles.AsNoTracking()
				.Where(x => x.UserId == userId)
				.Select(x => x.RoleId)
				.ToArrayAsync(cancellationToken),
			cancellationToken: cancellationToken);
	}

	public ValueTask InvalidateAsync(Guid userId, CancellationToken cancellationToken = default) =>
		cache.RemoveAsync(GetCacheKey(userId), cancellationToken);

	private static string GetCacheKey(Guid userId) => $"iam:user-roles:{userId}";
}
