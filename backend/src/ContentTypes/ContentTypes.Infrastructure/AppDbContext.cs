using Application.Abstractions;
using Application.Abstractions.IntegrationEvents;
using ContentTypes.Core;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace ContentTypes.Infrastructure;

public class AppDbContext(
	DbContextOptions<AppDbContext> options,
	IDomainEventPublisher publisher,
	IEnumerable<IIntegrationEventCollector> collectors)
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


		await publisher.PublishAsync(domainEvents, cancellationToken);

		var dispatchTasks = collectors
			.SelectMany(c => c.Collect(domainEvents)
				.Select(ev => c.DispatchAsync(ev.EventName, ev.Payload, cancellationToken)));

		await Task.WhenAll(dispatchTasks);


		return result;
	}
}