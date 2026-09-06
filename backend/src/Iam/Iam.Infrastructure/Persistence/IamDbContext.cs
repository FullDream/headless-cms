using Iam.Domain.AccessResource;
using Iam.Infrastructure.Roles;
using Iam.Infrastructure.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Iam.Infrastructure.Persistence;

internal sealed class IamDbContext(DbContextOptions<IamDbContext> options)
	: IdentityDbContext<PersistenceUser, PersistenceRole, Guid>(options)
{
	internal DbSet<AccessResource> Resources => Set<AccessResource>();

	internal DbSet<PersistencePermission> RolePermissions => Set<PersistencePermission>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.ApplyConfigurationsFromAssembly(typeof(IamDbContext).Assembly);
	}
}
