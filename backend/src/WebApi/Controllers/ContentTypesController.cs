using Application.ContentTypes.Commands.AddField;
using Application.ContentTypes.Commands.Create;
using Application.ContentTypes.Commands.Remove;
using Application.ContentTypes.Commands.RemoveField;
using Application.ContentTypes.Commands.Update;
using Application.ContentTypes.Commands.UpdateField;
using Application.ContentTypes.Dtos;
using Application.ContentTypes.Queries.GetAll;
using Application.ContentTypes.Queries.GetById;
using Application.ContentTypes.Queries.GetByName;
using Core.ContentTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using WebApi.Common.Results;
using WebApi.Contracts.ContentTypes;

namespace WebApi.Controllers;

[ApiController]
[Route("content-types")]
public class ContentTypesController(IMediator mediator, ProblemDetailsFactory detailsService) : ControllerBase
{
	[HttpGet]
	public async Task<OutcomeResult<IEnumerable<ContentTypeDto>>> Index(
		[FromQuery] ContentTypeKind? kind,
		CancellationToken cancellationToken)
		=> await mediator.Send(new GetContentTypesQuery(kind), cancellationToken);

	[HttpGet("{id:guid}")]
	public async Task<OutcomeResult<ContentTypeDto>> GetById(Guid id, CancellationToken cancellationToken) =>
		await mediator.Send(new GetContentTypeByIdQuery(id), cancellationToken);

	[HttpGet("{name}")]
	public async Task<OutcomeResult<ContentTypeDto>> GetByName(string name, CancellationToken cancellationToken) =>
		await mediator.Send(new GetContentTypeByNameQuery(name), cancellationToken);

	[HttpPost]
	public async Task<OutcomeResult<ContentTypeDto>> Create(CreateContentTypeDto body,
		CancellationToken cancellationToken)
		=> await mediator.Send(new CreateContentTypeCommand(body.Name, body.Kind, body.Fields), cancellationToken);

	[HttpPatch("{id:guid}")]
	public async Task<OutcomeResult<ContentTypeDto>> Update(Guid id, UpdateContentTypeDto body,
		CancellationToken cancellationToken) =>
		await mediator.Send(new UpdateContentTypeCommand(id, body.Name, body.Kind), cancellationToken);

	[HttpDelete("{id:guid}")]
	public async Task<OutcomeResult<ContentTypeDto>> Delete(Guid id, CancellationToken cancellationToken) =>
		await mediator.Send(new RemoveContentTypeCommand(id), cancellationToken);

	[HttpPost("{id:guid}/fields")]
	public async Task<OutcomeResult<ContentFieldDto>> AddField(Guid id, CreateContentFieldDto field,
		CancellationToken cancellationToken) =>
		await mediator.Send(new AddFieldToContentTypeCommand(id, field), cancellationToken);

	[HttpPatch("{id:guid}/fields/{fieldId:guid}")]
	public async Task<OutcomeResult<ContentFieldDto>> UpdateField(Guid id, Guid fieldId,
		UpdateContentFieldDto updateDto, CancellationToken cancellationToken) =>
		await mediator.Send(new UpdateFieldInContentTypeCommand(id, fieldId, updateDto), cancellationToken);

	[HttpDelete("{id:guid}/fields/{fieldId:guid}")]
	public async Task<OutcomeResult<ContentFieldDto>> RemoveField(Guid id, Guid fieldId,
		CancellationToken cancellationToken) =>
		await mediator.Send(new RemoveFieldFromContentTypeCommand(id, fieldId), cancellationToken);
}