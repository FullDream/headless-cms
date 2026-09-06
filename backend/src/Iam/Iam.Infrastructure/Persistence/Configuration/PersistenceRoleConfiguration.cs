using Iam.Domain.Roles;
using Iam.Infrastructure.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iam.Infrastructure.Persistence.Configuration;

internal sealed class PersistenceRoleConfiguration : IEntityTypeConfiguration<PersistenceRole>
{
	public void Configure(EntityTypeBuilder<PersistenceRole> builder) =>
		builder.Property(x => x.Kind).HasConversion<string>().HasDefaultValue(RoleKind.Custom);
}
