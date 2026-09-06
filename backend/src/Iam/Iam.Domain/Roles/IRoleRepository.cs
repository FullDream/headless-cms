namespace Iam.Domain.Roles;

public interface IRoleRepository
{
	Task<IReadOnlyCollection<Role>> FindManyAsync(CancellationToken cancellationToken = default);

	Task<Role?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

	Task<Role?> FindByNameAsync(string name, CancellationToken cancellationToken = default);

	Task AddAsync(Role role, CancellationToken cancellationToken = default);

	Task UpdateAsync(Role role, CancellationToken cancellationToken = default);

	Task RemoveAsync(Role role, CancellationToken cancellationToken = default);
}
