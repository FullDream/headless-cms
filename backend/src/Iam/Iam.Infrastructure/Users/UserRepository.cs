using Iam.Domain.Users;
using Iam.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Iam.Infrastructure.Users;

internal sealed class UserRepository(UserManager<PersistenceUser> userManager, IamDbContext dbContext) : IUserRepository
{
	public async Task<IReadOnlyCollection<User>> FindManyAsync(CancellationToken cancellationToken = default)
	{
		var users = await userManager
			.Users
			.AsNoTracking()
			.Select(user => new
			{
				user.Id,
				user.Email,
				RoleIds = dbContext
					.UserRoles
					.Where(userRole => userRole.UserId == user.Id)
					.Select(userRole => userRole.RoleId)
					.ToArray()
			})
			.ToArrayAsync(cancellationToken);

		return users
			.Select(user => User.Restore(
				user.Id,
				user.Email ?? throw new InvalidOperationException($"User '{user.Id}' has no email."),
				user.RoleIds))
			.ToArray();
	}

	public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var user = await userManager
			.Users
			.AsNoTracking()
			.Where(user => user.Id == id)
			.Select(user => new
			{
				user.Id,
				user.Email,
				RoleIds = dbContext
					.UserRoles
					.Where(userRole => userRole.UserId == user.Id)
					.Select(userRole => userRole.RoleId)
					.ToArray()
			})
			.SingleOrDefaultAsync(cancellationToken);

		return user is null
			? null
			: User.Restore(
				user.Id,
				user.Email ?? throw new InvalidOperationException($"User '{user.Id}' has no email."),
				user.RoleIds);
	}

	public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
	{
		var currentUserRoles = await dbContext
			.UserRoles
			.Where(userRole => userRole.UserId == user.Id)
			.ToArrayAsync(cancellationToken);

		var currentRoleIds = currentUserRoles.Select(userRole => userRole.RoleId).ToHashSet();

		var roleIds = user.RoleIds.ToHashSet();

		var userRolesToRemove = currentUserRoles.Where(userRole => !roleIds.Contains(userRole.RoleId)).ToArray();

		var roleIdsToAdd = roleIds.Where(roleId => !currentRoleIds.Contains(roleId)).ToArray();

		dbContext.UserRoles.RemoveRange(userRolesToRemove);

		foreach (var roleId in roleIdsToAdd)
		{
			dbContext.UserRoles.Add(
				new IdentityUserRole<Guid>
				{
					UserId = user.Id,
					RoleId = roleId,
				});
		}

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
