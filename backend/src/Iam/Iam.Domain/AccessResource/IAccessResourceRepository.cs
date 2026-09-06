namespace Iam.Domain.AccessResource;

public interface IAccessResourceRepository
{
	Task<IReadOnlyCollection<AccessResource>> FindManyAsync(CancellationToken cancellationToken = default);

	Task<AccessResource?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

	Task<AccessResource?> FindByKeyAsync(string key, CancellationToken cancellationToken = default);

	void Add(AccessResource resource);

	void Remove(AccessResource resource);

	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
