using ContentTypes.Core;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using IDomainEventPublisher = Application.Abstractions.IntegrationEvents.IDomainEventPublisher;

namespace ContentTypes.Infrastructure;

public class AppDbContext(
	DbContextOptions<AppDbContext> options,
	IEnumerable<IDomainEventPublisher> collectors)
	: DbContext(options)
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
		var domainEvents = ChangeTracker
			.Entries<AggregateRoot>()
			.SelectMany(e => e.Entity.DomainEvents)
			.ToList();


		var result = await base.SaveChangesAsync(cancellationToken);

		ChangeTracker.Entries<AggregateRoot>().ToList().ForEach(entity => entity.Entity.ClearDomainEvents());

		if (domainEvents.Count == 0)
			return result;

		var dispatchTasks = collectors
			.SelectMany(c => c.Collect(domainEvents)
				.Select(@event => c.DispatchAsync(@event, cancellationToken)));

		await Task.WhenAll(dispatchTasks);


		return result;
	}
}