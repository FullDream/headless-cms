namespace BuildingBlocks;

public interface IEventDispatcher<TTag>
{
	Task DispatchAsync<T>(string eventName, T dto, CancellationToken cancellationToken = default);
}