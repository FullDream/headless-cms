using ContentTypes.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContentTypes.Infrastructure;

public class ContentFieldConfiguration : IEntityTypeConfiguration<ContentField>
{
	public void Configure(EntityTypeBuilder<ContentField> builder)
	{
		builder.HasKey(f => f.Id);

		builder.Property(f => f.Name).IsRequired();
		builder.Property(f => f.IsRequired).IsRequired();

		builder.Property(f => f.Type)
			.HasConversion<string>()
			.IsRequired();

		builder.Property(f => f.Order).IsRequired();
	}
}