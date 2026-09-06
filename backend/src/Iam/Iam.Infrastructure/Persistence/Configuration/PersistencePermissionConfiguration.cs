using Iam.Domain.AccessResource;
using Iam.Infrastructure.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iam.Infrastructure.Persistence.Configuration;

internal sealed class PersistencePermissionConfiguration : IEntityTypeConfiguration<PersistencePermission>
{
	public void Configure(EntityTypeBuilder<PersistencePermission> builder)
	{
		builder.ToTable("iam_role_permissions");

		builder.HasKey(x => new
		{
			x.RoleId,
			x.ResourceId,
			x.Action
		});

		builder.Property(x => x.Action).HasConversion<string>();

		builder.Property(x => x.Scope).HasConversion<string>();

		builder.HasOne<PersistenceRole>()
			.WithMany(x => x.Permissions)
			.HasForeignKey(x => x.RoleId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne<AccessResource>().WithMany().HasForeignKey(x => x.ResourceId).OnDelete(DeleteBehavior.Restrict);
	}
}
