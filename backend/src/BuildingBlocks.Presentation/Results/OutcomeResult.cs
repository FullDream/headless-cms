using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using SharedKernel.Result;

namespace BuildingBlocks.Presentation.Results;

public class OutcomeResult(Result result) : ActionResult, IResult
{
	protected Result InnerResult { get; } = result;

	// Preserve existing formatter/customization behavior when the host opted into MVC.
	// A Minimal-only host requires no MVC services and uses native HTTP results.
	public Task ExecuteAsync(HttpContext httpContext) =>
		httpContext.RequestServices.GetService<IActionResultExecutor<ObjectResult>>() is not null
			? ExecuteResultAsync(new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor()))
			: ExecuteMinimalAsync(httpContext);

	protected virtual Task ExecuteMinimalAsync(HttpContext context) =>
		InnerResult.IsSuccess
			? TypedResults.NoContent().ExecuteAsync(context)
			: ExecuteMinimalFailure(context, InnerResult.Errors!);

	protected static Task ExecuteMinimalFailure(HttpContext context, Error[] errors)
	{
		var problem = ResultProblemDetailsMapper.CreateProblemDetails(context, errors);
		return TypedResults
			.Json(problem, statusCode: problem.Status, contentType: "application/problem+json; charset=utf-8")
			.ExecuteAsync(context);
	}

	public override Task ExecuteResultAsync(ActionContext context)
	{
		if (InnerResult.IsSuccess) return new NoContentResult().ExecuteResultAsync(context);

		return ExecuteFailure(context, InnerResult.Errors!);
	}

	protected static Task ExecuteFailure(ActionContext context, Error[] errors)
	{
		var problemDetails = ResultProblemDetailsMapper.CreateProblemDetails(context.HttpContext, errors);

		return new ObjectResult(problemDetails).ExecuteResultAsync(context);
	}

	public static implicit operator OutcomeResult(Result innerResult) => new(innerResult);
}
