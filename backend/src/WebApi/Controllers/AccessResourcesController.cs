using Iam.Application.AccessResources;
using Iam.Application.AccessResources.GetAccessResources;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Results;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("access-resources")]
public class AccessResourcesController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<OutcomeResult<IReadOnlyCollection<AccessResourceDto>>>
		Index(CancellationToken cancellationToken) =>
		await mediator.Send(new GetAccessResourcesQuery(), cancellationToken);
}
