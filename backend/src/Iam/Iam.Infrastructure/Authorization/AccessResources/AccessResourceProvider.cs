using Iam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Iam.Infrastructure.Authorization.AccessResources;

internal sealed class AccessResourceProvider(IamDbContext dbContext, HybridCache cache)
{
	private const string CacheKey = "iam:access-resources";

	public async Task<IReadOnlyDictionary<string, AccessResourceSnapshot>> GetAsync(
		CancellationToken cancellationToken = default)
	{
		return await cache.GetOrCreateAsync(
			CacheKey,
			async cancellationToken =>
			{
				var resources = await dbContext.Resources.AsNoTracking()
					.Select(x => new AccessResourceSnapshot(x.Id, x.Key))
					.ToArrayAsync(cancellationToken);

				return resources.ToDictionary(x => x.Key);
			},
			cancellationToken: cancellationToken);
	}

	public ValueTask InvalidateAsync(CancellationToken cancellationToken = default) =>
		cache.RemoveAsync(CacheKey, cancellationToken);
}
