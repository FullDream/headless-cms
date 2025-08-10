using System.Text.Json;
using Application.ContentEntries.Commands.Create;
using Application.ContentEntries.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("content-entries/{name}")]
public class ContentEntriesController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IReadOnlyList<Dictionary<string, object>>>> Index(string name)
	{
		var items = await mediator.Send(new ListContentEntriesQuery(name));

		return Ok(items);
	}

	[HttpPost]
	public async Task<ActionResult<Dictionary<string, object?>>> CreateEntry(string name,
		[FromBody] Dictionary<string, JsonElement> body, CancellationToken cancellationToken)
	{
		var item = await mediator.Send(new CreateContentEntryCommand(name, body), cancellationToken);

		return Ok(item);
	}
}