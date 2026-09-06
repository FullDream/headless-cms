using Iam.Domain.AccessResource;
using Iam.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Iam.Infrastructure.AccessResources;

internal sealed class AccessResourceRepository(IamDbContext dbContext) : IAccessResourceRepository
{
	public async Task<IReadOnlyCollection<AccessResource>>
		FindManyAsync(CancellationToken cancellationToken = default) =>
		await dbContext.Resources.AsNoTracking()
			.Include(x => x.Capabilities)
			.OrderBy(x => x.Name)
			.ToArrayAsync(cancellationToken);

	public Task<AccessResource?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
		dbContext.Resources.Include(x => x.Capabilities).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

	public Task<AccessResource?> FindByKeyAsync(string key, CancellationToken cancellationToken = default) =>
		dbContext.Resources.Include(x => x.Capabilities).FirstOrDefaultAsync(x => x.Key == key, cancellationToken);

	public void Add(AccessResource resource) => dbContext.Resources.Add(resource);

	public void Remove(AccessResource resource) => dbContext.Resources.Remove(resource);

	public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
		dbContext.SaveChangesAsync(cancellationToken);
}
