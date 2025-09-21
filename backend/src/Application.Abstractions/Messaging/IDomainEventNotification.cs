using MediatR;
using SharedKernel.Events;

namespace Application.Abstractions.Messaging;

public record DomainEventNotification<T>(T DomainEvent) : INotification
	where T : IDomainEvent;