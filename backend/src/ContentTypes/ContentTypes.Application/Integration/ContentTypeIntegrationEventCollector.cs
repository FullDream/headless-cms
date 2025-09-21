using Application.Abstractions;
using Application.Abstractions.IntegrationEvents;
using Application.Abstractions.IntegrationEvents.Tags;
using ContentTypes.Application.Mappers;
using ContentTypes.Core;
using ContentTypes.Core.Events;
using SharedKernel.Events;

namespace ContentTypes.Application.Integration;

public class ContentTypeIntegrationEventCollector(IEventDispatcher<ContentTypeEventTag> dispatcher)
	: IntegrationEventCollectorBase<ContentType>
{
	private static readonly (string EventName, Func<ContentType, object> ToPayload, Func<IDomainEvent, bool> Match)[]
		Rules =
		[
			("removed", ct => ct.ToDto(), e => e is ContentTypeRemovedEvent),
			("created", ct => ct.ToDto(), e => e is ContentTypeCreatedEvent),
			("updated", ct => ct.ToDto(), _ => true)
		];

	public override Task DispatchAsync(string eventName, object payload, CancellationToken cancellationToken) =>
		dispatcher.DispatchAsync(eventName, payload, cancellationToken);

	protected override IEnumerable<(string EventName, object Payload)> CollectTyped(
		IEnumerable<IDomainEvent<ContentType>> events)
		=> from groupedEvents in events.GroupBy(e => e.AggregateRoot.Id)
			let aggregate = groupedEvents.First().AggregateRoot
			let rule = Rules.First(r => groupedEvents.Any(r.Match))
			select (rule.EventName, rule.ToPayload(aggregate));
}