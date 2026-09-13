namespace Iam.Domain.Users;

public interface IUserRepository
{
	Task<IReadOnlyCollection<User>> FindManyAsync(CancellationToken cancellationToken = default);

	Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

	Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}
