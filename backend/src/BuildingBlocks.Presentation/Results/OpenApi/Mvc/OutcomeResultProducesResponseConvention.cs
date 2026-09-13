using BuildingBlocks.Presentation.Results.OpenApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

// Preserve the public namespace for existing MVC registrations.
namespace BuildingBlocks.Presentation.Results;

public class OutcomeResultProducesResponseConvention : IApplicationModelConvention
{
	public void Apply(ApplicationModel app)
	{
		foreach (var action in app.Controllers.SelectMany(c => c.Actions))
		{
			var methods = action.Selectors.SelectMany(s =>
				s.ActionConstraints?.OfType<HttpMethodActionConstraint>().SelectMany(c => c.HttpMethods) ?? []);
			// Preserve the old convention's action-selector semantics for MVC.
			var hasPlaceholders = action.Selectors.Any(s =>
				s.AttributeRouteModel?.Template is { } template && (template.Contains('{') || template.Contains('}')));
			var responses = OutcomeResultResponseMetadata.GetResponses(
				action.ActionMethod.ReturnType,
				methods,
				hasPlaceholders);

			foreach (var response in responses)
			{
				if (action
				    .Filters
				    .OfType<ProducesResponseTypeAttribute>()
				    .Any(a => a.StatusCode == response.StatusCode))
					continue;

				action.Filters.Add(
					response.ContentType is null
						? new ProducesResponseTypeAttribute(response.StatusCode)
						: new ProducesResponseTypeAttribute(response.Type, response.StatusCode, response.ContentType));
			}
		}
	}
}
