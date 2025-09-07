namespace Application.Abstractions;

public interface IEventDispatcher
{
	Task DispatchAsync<T>(string eventName, T dto, CancellationToken cancellationToken = default);
}