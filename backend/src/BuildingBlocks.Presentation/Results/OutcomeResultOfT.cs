using Microsoft.AspNetCore.Mvc;
using SharedKernel.Result;

namespace BuildingBlocks.Presentation.Results;

public class OutcomeResult<T>(Result<T> result) : OutcomeResult(result)
{
	protected override Task ExecuteMinimalAsync(HttpContext context)
	{
		if (result.IsFailure) return ExecuteMinimalFailure(context, result.Errors!);

		// Match the default MVC null-output behavior.
		return result.Value is null
			? TypedResults.NoContent().ExecuteAsync(context)
			: TypedResults.Ok(result.Value).ExecuteAsync(context);
	}

	public override Task ExecuteResultAsync(ActionContext context)
	{
		if (result.IsSuccess) return new OkObjectResult(result.Value).ExecuteResultAsync(context);

		return ExecuteFailure(context, result.Errors!);
	}

	public static implicit operator OutcomeResult<T>(Result<T> innerResult) => new(innerResult);
}
