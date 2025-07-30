using Application.ContentTypes.Commands.AddFieldToContentType;
using Application.ContentTypes.Commands.CreateContentType;
using Application.ContentTypes.Commands.RemoveContentType;
using Application.ContentTypes.Commands.RemoveFieldFromContentType;
using Application.ContentTypes.Commands.UpdateContentType;
using Application.ContentTypes.Commands.UpdateFieldInContentType;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Queries.GetContentTypeByName;
using Application.ContentTypes.Queries.GetContentTypes;
using Core.ContentTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.ContentTypes;

namespace WebApi.Controllers;

[ApiController]
[Route("content-types")]
public class ContentTypesController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<List<ContentTypeDto>>> Index([FromQuery] ContentTypeKind? kind,
		CancellationToken cancellationToken)
	{
		var result = await mediator.Send(new GetContentTypesQuery(kind), cancellationToken);

		return Ok(result);
	}

	[HttpGet("{name}")]
	public async Task<ActionResult<ContentTypeDto>> GetByName(string name, CancellationToken cancellationToken)
	{
		var item = await mediator.Send(new GetContentTypeByNameQuery(name), cancellationToken);

		return Ok(item);
	}

	[HttpPost]
	public async Task<ActionResult<ContentTypeDto>> Create([FromBody] CreateContentTypeCommand command,
		CancellationToken cancellationToken)
	{
		var item = await mediator.Send(command, cancellationToken);

		return Ok(item);
	}

	[HttpPatch("{id:guid}")]
	public async Task<ActionResult<ContentTypeDto>> Update(Guid id, [FromBody] UpdateContentTypeDto body,
		CancellationToken cancellationToken)
	{
		var item = await mediator.Send(new UpdateContentTypeCommand(id, body.Name, body.Kind), cancellationToken);

		return Ok(item);
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult<ContentTypeDto?>> Delete(Guid id, CancellationToken cancellationToken)
	{
		var item = await mediator.Send(new RemoveContentTypeCommand(id), cancellationToken);

		return Ok(item);
	}

	[HttpPost("{id:guid}/fields")]
	public async Task<ActionResult<ContentFieldDto>> AddField(Guid id, [FromBody] CreateContentFieldDto field)
	{
		var item = await mediator.Send(new AddFieldToContentTypeCommand(id, field));

		return Ok(item);
	}

	[HttpPatch("{id:guid}/fields/{fieldId:guid}")]
	public async Task<ActionResult<ContentFieldDto>> UpdateField(Guid id, Guid fieldId,
		[FromBody] UpdateContentFieldDto updateDto)
	{
		var item = await mediator.Send(new UpdateFieldInContentTypeCommand(id, fieldId, updateDto));

		return Ok(item);
	}

	[HttpDelete("{id:guid}/fields/{fieldId:guid}")]
	public async Task<ActionResult<ContentFieldDto>> RemoveField(Guid id, Guid fieldId)
	{
		var item = await mediator.Send(new RemoveFieldFromContentTypeCommand(id, fieldId));

		if (item is null) return NotFound();

		return Ok(item);
	}
}