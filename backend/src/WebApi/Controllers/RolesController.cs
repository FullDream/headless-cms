using Iam.Application.Roles;
using Iam.Application.Roles.CreateRole;
using Iam.Application.Roles.GetRoleById;
using Iam.Application.Roles.GetRoles;
using Iam.Application.Roles.RemoveRole;
using Iam.Application.Roles.UpdateRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Results;
using WebApi.Contracts.Roles;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("roles")]
public class RolesController(IMediator mediator) : ControllerBase
{
	[HttpGet(Name = "Roles")]
	public async Task<OutcomeResult<IReadOnlyCollection<RoleDto>>> Index(CancellationToken cancellationToken) =>
		await mediator.Send(new GetRolesQuery(), cancellationToken);

	[HttpGet("{id:guid}", Name = "RoleById")]
	public async Task<OutcomeResult<RoleDto>> GetById(Guid id, CancellationToken cancellationToken) =>
		await mediator.Send(new GetRoleByIdQuery(id), cancellationToken);

	[HttpPost(Name = "CreateRole")]
	public async Task<OutcomeResult<RoleDto>> Create(CreateRoleDto body, CancellationToken cancellationToken) =>
		await mediator.Send(new CreateRoleCommand(body.Name, body.Permissions), cancellationToken);

	[HttpPut("{id:guid}", Name = "UpdateRole")]
	public async Task<OutcomeResult<RoleDto>>
		Update(Guid id, UpdateRoleDto body, CancellationToken cancellationToken) =>
		await mediator.Send(new UpdateRoleCommand(id, body.Name, body.Permissions), cancellationToken);

	[HttpDelete("{id:guid}", Name = "DeleteRole")]
	public async Task<OutcomeResult<RoleDto>> Delete(Guid id, CancellationToken cancellationToken) =>
		await mediator.Send(new RemoveRoleCommand(id), cancellationToken);
}
