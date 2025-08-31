using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using WebApi.Results;

namespace WebApi.Conventions;

public class OutcomeResultProducesResponseConvention : IApplicationModelConvention
{
	private const string ValidationProblemDetailsContentType = "application/problem+json";
	private static readonly Type ValidationProblemDetailsType = typeof(ValidationProblemDetails);
	private static readonly Type ProblemDetailsType = typeof(ValidationProblemDetails);

	private static readonly Dictionary<int, Type> ErrorResponses = new()
	{
		[StatusCodes.Status400BadRequest] = ValidationProblemDetailsType,
		[StatusCodes.Status401Unauthorized] = ProblemDetailsType,
		[StatusCodes.Status403Forbidden] = ProblemDetailsType,
		[StatusCodes.Status404NotFound] = ProblemDetailsType,
		[StatusCodes.Status409Conflict] = ValidationProblemDetailsType,
		[StatusCodes.Status422UnprocessableEntity] = ProblemDetailsType,
	};

	public void Apply(ApplicationModel app)
	{
		foreach (var action in app.Controllers.SelectMany(c => c.Actions))
		{
			var returnType = UnwrapTask(action.ActionMethod.ReturnType);

			if (returnType?.IsGenericType == true &&
			    returnType.GetGenericTypeDefinition() == typeof(OutcomeResult<>))
			{
				var payloadType = returnType.GetGenericArguments()[0];

				if (!HasProduces(action, StatusCodes.Status200OK))
				{
					action.Filters.Add(new ProducesResponseTypeAttribute(
						payloadType,
						StatusCodes.Status200OK,
						"application/json"));
				}
			}

			foreach (var (status, problemType) in ErrorResponses)
				if (!HasProduces(action, status))
					action.Filters.Add(new ProducesResponseTypeAttribute(
						problemType,
						status,
						"application/problem+json"));
		}
	}

	private static bool HasProduces(ActionModel action, int status) =>
		action.Filters.OfType<ProducesResponseTypeAttribute>()
			.Any(a => a.StatusCode == status);

	private static Type? UnwrapTask(Type? type)
	{
		if (type is null) return null;
		if (type.IsGenericType)
		{
			var g = type.GetGenericTypeDefinition();
			if (g == typeof(Task<>) || g == typeof(ValueTask<>))
				return type.GetGenericArguments()[0];
		}

		return type;
	}
}