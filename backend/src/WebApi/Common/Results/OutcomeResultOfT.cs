using Microsoft.AspNetCore.Mvc;
using SharedKernel.Result;

namespace WebApi.Common.Results;

public class OutcomeResult<T>(Result<T> result) : OutcomeResult(result)
{
	public override Task ExecuteResultAsync(ActionContext context)
	{
		if (result.IsSuccess) return new OkObjectResult(result.Value).ExecuteResultAsync(context);

		return ExecuteFailure(context, result.Errors!);
	}

	public static implicit operator OutcomeResult<T>(Result<T> innerResult) => new(innerResult);
}