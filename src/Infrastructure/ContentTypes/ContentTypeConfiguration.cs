using Core.ContentTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ContentTypes;

public class ContentTypeConfiguration: IEntityTypeConfiguration<ContentType>
{

	public void Configure(EntityTypeBuilder<ContentType> builder)
	{
		builder.HasKey(ct => ct.Id);

		builder.Property(ct => ct.Name).IsRequired();
		builder.HasIndex(ct => ct.Name).IsUnique();

		builder.Property(ct => ct.Kind).IsRequired();
	}
	
}