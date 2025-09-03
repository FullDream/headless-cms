using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebApi.Common.Results;

public class OutcomeResultProducesResponseConvention : IApplicationModelConvention
{
	private static readonly Type ValidationProblemDetailsType = typeof(ValidationProblemDetails);
	private static readonly Type ProblemDetailsType = typeof(ProblemDetails);

	private static readonly Dictionary<int, Type> ErrorResponses = new()
	{
		[StatusCodes.Status400BadRequest] = ValidationProblemDetailsType,
		[StatusCodes.Status401Unauthorized] = ProblemDetailsType,
		[StatusCodes.Status403Forbidden] = ProblemDetailsType,
		[StatusCodes.Status404NotFound] = ProblemDetailsType,
		[StatusCodes.Status409Conflict] = ValidationProblemDetailsType,
		[StatusCodes.Status422UnprocessableEntity] = ValidationProblemDetailsType,
	};

	public void Apply(ApplicationModel app)
	{
		foreach (var action in app.Controllers.SelectMany(c => c.Actions))
		{
			var returnType = UnwrapTask(action.ActionMethod.ReturnType);
			if (returnType?.IsGenericType == false || returnType?.GetGenericTypeDefinition() != typeof(OutcomeResult<>))
				continue;

			var payloadType = returnType.GetGenericArguments()[0];

			if (NeedsProduces(action, StatusCodes.Status200OK))
				action.Filters.Add(new ProducesResponseTypeAttribute(
					payloadType,
					StatusCodes.Status200OK,
					"application/json"));


			var errorStatuses = ErrorResponses.Keys.ToList();

			if (IsHttpMethod(action, "GET"))
			{
				errorStatuses.Remove(StatusCodes.Status409Conflict);

				if (HasNoRoutePlaceholders(action) && IsListGet(action, payloadType))
					errorStatuses.Remove(StatusCodes.Status404NotFound);
			}

			else if (IsHttpMethod(action, "POST") && HasNoRoutePlaceholders(action))
				errorStatuses.Remove(StatusCodes.Status404NotFound);

			else if (IsHttpMethod(action, "DELETE"))
			{
				errorStatuses.Remove(StatusCodes.Status422UnprocessableEntity);
				errorStatuses.Remove(StatusCodes.Status409Conflict);
			}

			foreach (var status in errorStatuses.Where(status => NeedsProduces(action, status)))
				action.Filters.Add(new ProducesResponseTypeAttribute(
					ErrorResponses[status],
					status,
					"application/problem+json"));
		}
	}

	private static bool IsHttpMethod(ActionModel action, string method)
	{
		return action.Selectors.Any(s =>
			s.ActionConstraints?.OfType<HttpMethodActionConstraint>()
				.Any(c => c.HttpMethods.Contains(method, StringComparer.OrdinalIgnoreCase)) == true);
	}

	private static bool HasNoRoutePlaceholders(ActionModel action) =>
		action.Selectors.All(s =>
			string.IsNullOrEmpty(s.AttributeRouteModel?.Template) ||
			(!s.AttributeRouteModel.Template.Contains('{') && !s.AttributeRouteModel.Template.Contains('}')));

	private static bool IsListGet(ActionModel action, Type payload)
	{
		return
			payload.IsArray ||
			(payload.IsGenericType && (payload.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
			                           payload.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>))) ||
			typeof(IEnumerable).IsAssignableFrom(payload);
	}

	private static bool NeedsProduces(ActionModel action, int status) =>
		action.Filters.OfType<ProducesResponseTypeAttribute>().All(a => a.StatusCode != status);

	private static Type? UnwrapTask(Type? type)
	{
		if (type is null) return null;
		if (type.IsGenericType)
		{
			var genericTypeDefinition = type.GetGenericTypeDefinition();
			if (genericTypeDefinition == typeof(Task<>) || genericTypeDefinition == typeof(ValueTask<>))
				return type.GetGenericArguments()[0];
		}

		return type;
	}
}