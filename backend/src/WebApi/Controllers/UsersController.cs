using Iam.Application.Users;
using Iam.Application.Users.GetUserById;
using Iam.Application.Users.GetUsers;
using Iam.Application.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Results;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("users")]
public sealed class UsersController(IMediator mediator) : ControllerBase
{
	[HttpGet(Name = "Users")]
	public async Task<OutcomeResult<IReadOnlyCollection<UserDto>>> Index() => await mediator.Send(new GetUsersQuery());

	[HttpGet("{id:guid}", Name = "UserById")]
	public async Task<OutcomeResult<UserDto>> GetById(Guid id) => await mediator.Send(new GetUserByIdQuery(id));

	[HttpPatch("{id:guid}", Name = "UpdateUser")]
	public async Task<OutcomeResult<UserDto>> Update(Guid id, [FromBody] IReadOnlyCollection<Guid> roleIds) =>
		await mediator.Send(new UpdateUserCommand(id, roleIds));
}
