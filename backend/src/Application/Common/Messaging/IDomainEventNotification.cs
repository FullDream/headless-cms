using MediatR;
using SharedKernel.Events;

namespace Application.Common.Messaging;

public record DomainEventNotification<T>(T DomainEvent) : INotification
	where T : IDomainEvent;