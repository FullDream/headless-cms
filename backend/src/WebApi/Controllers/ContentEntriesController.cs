using System.Text.Json;
using ContentEntries.Application.Create;
using ContentEntries.Application.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Results;

namespace WebApi.Controllers;

[ApiController]
[Route("content-entries/{name}")]
public class ContentEntriesController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<OutcomeResult<IReadOnlyList<IReadOnlyDictionary<string, object?>>>> Index(string name) =>
		await mediator.Send(new ListContentEntriesQuery(name));

	[HttpPost]
	public async Task<OutcomeResult<IReadOnlyDictionary<string, object?>>> CreateEntry(string name,
		Dictionary<string, JsonElement> body, CancellationToken cancellationToken) =>
		await mediator.Send(new CreateContentEntryCommand(name, body), cancellationToken);
}