using BuildingBlocks;
using BuildingBlocks.IntegrationEvents;
using BuildingBlocks.IntegrationEvents.Tags;
using ContentTypes.Application.Dtos;
using ContentTypes.Application.Mappers;
using ContentTypes.Core;
using ContentTypes.Core.Events;
using SharedKernel.Events;

namespace ContentTypes.Application.Integration;

public class ContentTypeDomainEventPublisher(IEventDispatcher<ContentTypeEventTag> dispatcher)
	: DomainEventPublisherBase<ContentType, ContentTypeDto>
{
	private static readonly (string EventName, Func<ContentType, ContentTypeDto> ToPayload, Func<IDomainEvent, bool>
		Match)[]
		Rules =
		[
			("removed", ct => ct.ToDto(), e => e is ContentTypeRemovedEvent),
			("created", ct => ct.ToDto(), e => e is ContentTypeCreatedEvent),
			("updated", ct => ct.ToDto(), _ => true)
		];

	protected override Task DispatchAsync(
		IntegrationEvent<ContentTypeDto> ev,
		CancellationToken cancellationToken) =>
		dispatcher.DispatchAsync(ev.EventName, ev.Payload, cancellationToken);

	protected override IEnumerable<IntegrationEvent<ContentTypeDto>> CollectTyped(
		IEnumerable<IDomainEvent<ContentType>> events)
		=> from groupedEvents in events.GroupBy(e => e.AggregateRoot.Id)
			let aggregate = groupedEvents.First().AggregateRoot
			let rule = Rules.First(r => groupedEvents.Any(r.Match))
			select new IntegrationEvent<ContentTypeDto>(rule.EventName, rule.ToPayload(aggregate));
}