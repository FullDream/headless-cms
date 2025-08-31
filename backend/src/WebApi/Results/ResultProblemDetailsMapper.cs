using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SharedKernel.Result;

namespace WebApi.Results;

internal static class ResultProblemDetailsMapper
{
	internal static readonly ErrorType[] PriorityErrorTypes =
	[
		ErrorType.Unauthenticated,
		ErrorType.Forbidden,
		ErrorType.NotFound,
		ErrorType.Conflict,
		ErrorType.BusinessRule,
		ErrorType.Validation,
		ErrorType.Failure
	];


	internal static ProblemDetails CreateProblemDetails(ErrorType errorType, HttpContext httpContext,
		params Error[] errors)
	{
		var problemDetailsFactory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

		if (errorType == ErrorType.Validation)
			return problemDetailsFactory.CreateValidationProblemDetails(httpContext,
				BuildValidationErrors(errors),
				MapError(errorType));

		var problemDetails = problemDetailsFactory.CreateProblemDetails(httpContext,
			MapError(errorType),
			null,
			null,
			errors.FirstOrDefault()?.Message);

		if (errorType == ErrorType.Conflict)
			problemDetails.Extensions["errors"] = BuildValidationErrors(errors).ToDictionary(
				keyValuePair => keyValuePair.Key,
				keyValuePair => keyValuePair.Value!.Errors.Select(er => er.ErrorMessage).ToArray()
			);

		return problemDetails;
	}

	private static int MapError(ErrorType errorType) => errorType switch
	{
		ErrorType.Unauthenticated => StatusCodes.Status401Unauthorized,
		ErrorType.Forbidden => StatusCodes.Status403Forbidden,
		ErrorType.NotFound => StatusCodes.Status404NotFound,
		ErrorType.BusinessRule or ErrorType.Validation => StatusCodes.Status422UnprocessableEntity,
		ErrorType.Conflict => StatusCodes.Status409Conflict,
		_ => StatusCodes.Status400BadRequest
	};


	private static ModelStateDictionary BuildValidationErrors(IEnumerable<Error> errors)
	{
		var stateDictionary = new ModelStateDictionary();

		foreach (var e in errors)
			stateDictionary.AddModelError(string.IsNullOrWhiteSpace(e.Property) ? string.Empty : e.Property, e.Message);

		return stateDictionary;
	}
}