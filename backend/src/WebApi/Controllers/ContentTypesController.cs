using Application.ContentTypes.Commands.AddField;
using Application.ContentTypes.Commands.Create;
using Application.ContentTypes.Commands.Remove;
using Application.ContentTypes.Commands.RemoveField;
using Application.ContentTypes.Commands.Update;
using Application.ContentTypes.Commands.UpdateField;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Queries.GetAll;
using Application.ContentTypes.Queries.GetByName;
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

		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
	}

	[HttpGet("{name}")]
	public async Task<ActionResult<ContentTypeDto>> GetByName(string name, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(new GetContentTypeByNameQuery(name), cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
	}

	[HttpPost]
	public async Task<ActionResult<ContentTypeDto>> Create([FromBody] CreateContentTypeCommand command,
		CancellationToken cancellationToken)
	{
		var result = await mediator.Send(command, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
	}

	[HttpPatch("{id:guid}")]
	public async Task<ActionResult<ContentTypeDto>> Update(Guid id, [FromBody] UpdateContentTypeDto body,
		CancellationToken cancellationToken)
	{
		var result = await mediator.Send(new UpdateContentTypeCommand(id, body.Name, body.Kind), cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult<ContentTypeDto?>> Delete(Guid id, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(new RemoveContentTypeCommand(id), cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
	}

	[HttpPost("{id:guid}/fields")]
	public async Task<ActionResult<ContentFieldDto>> AddField(Guid id, [FromBody] CreateContentFieldDto field)
	{
		var result = await mediator.Send(new AddFieldToContentTypeCommand(id, field));

		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
	}

	[HttpPatch("{id:guid}/fields/{fieldId:guid}")]
	public async Task<ActionResult<ContentFieldDto>> UpdateField(Guid id, Guid fieldId,
		[FromBody] UpdateContentFieldDto updateDto)
	{
		var result = await mediator.Send(new UpdateFieldInContentTypeCommand(id, fieldId, updateDto));

		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
	}

	[HttpDelete("{id:guid}/fields/{fieldId:guid}")]
	public async Task<ActionResult<ContentFieldDto>> RemoveField(Guid id, Guid fieldId)
	{
		var result = await mediator.Send(new RemoveFieldFromContentTypeCommand(id, fieldId));

		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
	}
}