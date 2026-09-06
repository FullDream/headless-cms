using System.Text.Json;
using System.Text.Json.Serialization;
using Iam.Domain;
using Iam.Domain.AccessResource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Iam.Infrastructure.Persistence.Configuration;

internal sealed class AccessResourceConfiguration : IEntityTypeConfiguration<AccessResource>
{
	private static readonly JsonSerializerOptions jsonOptions = new()
	{
		Converters =
		{
			new JsonStringEnumConverter()
		}
	};

	public void Configure(EntityTypeBuilder<AccessResource> builder)
	{
		builder.ToTable("iam_access_resources");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id).ValueGeneratedNever();

		builder.Property(x => x.Key).HasMaxLength(100).IsRequired();

		builder.Property(x => x.Name).HasMaxLength(200).IsRequired();


		builder.HasOne<AccessResource>().WithMany().HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);

		builder.HasIndex(x => new { x.ParentId, x.Key }).IsUnique();

		builder.OwnsMany(
			x => x.Capabilities,
			capabilities =>
			{
				capabilities.ToTable("iam_resource_capabilities");

				capabilities.WithOwner().HasForeignKey("resource_id");

				capabilities.Property(x => x.Action).HasColumnName("action").HasConversion<string>().HasMaxLength(50);

				capabilities.HasKey("resource_id", nameof(ResourceCapability.Action));

				ConfigureAvailableScopes(capabilities);
			});

		builder.Navigation(x => x.Capabilities)
			.HasField("capabilities")
			.UsePropertyAccessMode(PropertyAccessMode.Field);
	}

	private static void ConfigureAvailableScopes(OwnedNavigationBuilder<AccessResource, ResourceCapability> builder)
	{
		var property = builder.Property<HashSet<AccessScope>>("availableScopes");

		property.HasColumnName("available_scopes")
			.HasConversion(
				scopes => JsonSerializer.Serialize(scopes, jsonOptions),
				value => JsonSerializer.Deserialize<HashSet<AccessScope>>(value, jsonOptions) ??
				         new HashSet<AccessScope>());

		property.Metadata.SetValueComparer(
			new ValueComparer<HashSet<AccessScope>>(
				(left, right) => left != null && right != null && left.SetEquals(right),
				scopes => scopes.OrderBy(x => x).Aggregate(0, HashCode.Combine),
				scopes => new HashSet<AccessScope>(scopes)));
	}
}
