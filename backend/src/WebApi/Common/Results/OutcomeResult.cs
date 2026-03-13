using Microsoft.AspNetCore.Mvc;
using SharedKernel.Result;

namespace WebApi.Common.Results;

public class OutcomeResult(Result result) : ActionResult
{
	protected Result InnerResult { get; } = result;

	public override Task ExecuteResultAsync(ActionContext context)
	{
		if (InnerResult.IsSuccess) return new NoContentResult().ExecuteResultAsync(context);

		return ExecuteFailure(context, InnerResult.Errors!);
	}

	protected static Task ExecuteFailure(ActionContext context, Error[] errors)
	{
		var winner =
			ResultProblemDetailsMapper.PriorityErrorTypes.First(errorType =>
				errors.Any(error => error.Type == errorType));

		var problemDetails = ResultProblemDetailsMapper.CreateProblemDetails(winner, context.HttpContext, errors);

		return new ObjectResult(problemDetails).ExecuteResultAsync(context);
	}

	public static implicit operator OutcomeResult(Result innerResult) => new(innerResult);
}