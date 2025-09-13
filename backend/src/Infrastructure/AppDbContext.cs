using Application.Abstractions;
using Core.ContentTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventPublisher publisher)
	: IdentityDbContext<IdentityUser>(options)
{
	public DbSet<ContentType> ContentTypes => Set<ContentType>();
	public DbSet<ContentField> ContentFields => Set<ContentField>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}


	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
	{
		var result = await base.SaveChangesAsync(cancellationToken);

		var domainEntities = ChangeTracker.Entries<AggregateRoot>().Where(x => x.Entity.DomainEvents.Count != 0)
			.ToList();

		var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();

		domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

		await publisher.PublishAsync(domainEvents, cancellationToken);

		return result;
	}
}