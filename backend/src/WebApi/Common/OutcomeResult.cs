using Microsoft.AspNetCore.Mvc;
using SharedKernel.Result;

namespace WebApi.Common;

public class OutcomeResult<T>(Result<T> result) : ActionResult
{
	public override Task ExecuteResultAsync(ActionContext context)
	{
		if (result.IsSuccess) return new OkObjectResult(result.Value).ExecuteResultAsync(context);

		var errors = result.Errors;
		var winner =
			ResultProblemDetailsMapper.PriorityErrorTypes.First(errorType =>
				errors.Any(error => error.Type == errorType));

		var problemDetails = ResultProblemDetailsMapper.CreateProblemDetails(winner, context.HttpContext, errors);

		return new ObjectResult(problemDetails).ExecuteResultAsync(context);
	}

	public static implicit operator OutcomeResult<T>(Result<T> innerResult) => new(innerResult);
}