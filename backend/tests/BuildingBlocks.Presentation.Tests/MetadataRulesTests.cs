using System.Collections;
using System.Reflection;
using BuildingBlocks.Presentation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace BuildingBlocks.Presentation.Tests;

public sealed class MetadataRulesTests
{
	[Theory]
	[InlineData(typeof(int[]), true)]
	[InlineData(typeof(IEnumerable<int>), true)]
	[InlineData(typeof(IAsyncEnumerable<int>), true)]
	[InlineData(typeof(IReadOnlyCollection<int>), true)]
	[InlineData(typeof(ArrayList), true)]
	[InlineData(typeof(string), true)] // Preserve the existing IEnumerable interpretation.
	[InlineData(typeof(Guid), false)]
	public void Original_collection_detection_is_preserved(Type payloadType, bool isCollection)
	{
		var method = typeof(Actions).GetMethod(nameof(Actions.Get))!.MakeGenericMethod(payloadType);
		var action = new ActionModel(method, []);
		var selector = new SelectorModel { AttributeRouteModel = new AttributeRouteModel(new RouteAttribute("")) };
		selector.ActionConstraints.Add(new HttpMethodActionConstraint(["GET"]));
		action.Selectors.Add(selector);
		var controller = new ControllerModel(typeof(Actions).GetTypeInfo(), []);
		controller.Actions.Add(action);
		var app = new ApplicationModel();
		app.Controllers.Add(controller);
		new OutcomeResultProducesResponseConvention().Apply(app);
		var responses = action.Filters.OfType<ProducesResponseTypeAttribute>().ToArray();
		Assert.Equal(!isCollection, responses.Any(response => response.StatusCode == 404));
		Assert.DoesNotContain(responses, response => response.StatusCode == 409);
		Assert.Equal(payloadType, responses.Single(response => response.StatusCode == 200).Type);
	}

	private static class Actions
	{
		public static OutcomeResult<T> Get<T>() => throw new NotSupportedException();
	}
}
