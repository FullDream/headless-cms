using ContentTypes.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentTypes.Infrastructure;

public class ContentTypeConfiguration : IEntityTypeConfiguration<ContentType>
{
	public void Configure(EntityTypeBuilder<ContentType> builder)
	{
		builder.HasKey(ct => ct.Id);

		builder.HasIndex(ct => ct.Name).IsUnique();
		builder.Property(ct => ct.Name).IsRequired();
		builder.Property(ct => ct.Kind).HasConversion<string>().IsRequired();


		builder.HasMany<ContentField>(ct => ct.Fields)
			.WithOne()
			.HasForeignKey(f => f.ContentTypeId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Navigation(ct => ct.Fields)
			.UsePropertyAccessMode(PropertyAccessMode.Field);

		builder.Ignore(ct => ct.DomainEvents);
	}
}