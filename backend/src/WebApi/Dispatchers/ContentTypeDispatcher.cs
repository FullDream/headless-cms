using Application.Abstractions;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Dispatchers;

public class ContentTypeDispatcher(IHubContext<ContentTypesHub> hub) : IEventDispatcher
{
	public Task DispatchAsync<T>(string eventName, T dto, CancellationToken cancellationToken = default)
		=> hub.Clients.All.SendAsync(eventName, dto, cancellationToken);
}