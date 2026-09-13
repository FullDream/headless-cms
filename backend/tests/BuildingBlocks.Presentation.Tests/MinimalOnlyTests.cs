using System.Text.Json;
using System.Text.Json.Serialization;
using BuildingBlocks.Presentation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Presentation.Tests;

public sealed class MinimalOnlyTests(OutcomeTestHost mvcHost) : IClassFixture<OutcomeTestHost>
{
	[Fact]
	public async Task Native_runtime_matches_existing_Mvc_contract_for_all_return_shapes_and_errors()
	{
		var builder = WebApplication.CreateBuilder();
		builder.WebHost.UseTestServer();
		builder.Logging.ClearProviders();
		builder.Services.ConfigureHttpJsonOptions(o =>
		{
			o.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
			o.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
		});
		builder.Services.AddProblemDetails(o => o.CustomizeProblemDetails = context =>
			context.ProblemDetails.Extensions["traceId"] = "outcome-contract-test");
		builder.Services.AddOpenApi();
		await using var app = builder.Build();
		Assert.Null(app.Services.GetService<ProblemDetailsFactory>());
		Assert.Null(app.Services.GetService<IActionResultExecutor<ObjectResult>>());
		Assert.Null(app.Services.GetService<IControllerFactory>());
		var api = app.MapGroup("/api").WithOutcomeResults();
		var users = api.MapGroup("/users");
		users.MapGet("/direct", Handlers.Direct);
		users.MapGet("/task", Handlers.TaskResult);
		users.MapGet("/value-task", Handlers.ValueTaskResult);
		users.MapGet("/direct-value", Handlers.DirectValue);
		users.MapGet("/task-value", Handlers.TaskValue);
		users.MapGet("/value-task-value", Handlers.ValueTaskValue);
		users.MapGet("/explicit", Handlers.DirectValue).Produces<string>(200, "text/plain");
		app.MapOpenApi();
		await app.StartAsync();
		using var client = app.GetTestClient();
		foreach (var row in RuntimeTests.RuntimeCases())
		{
			var shape = (string)row[0];
			var scenario = (string)row[1];
			using var mvc = await mvcHost.Client.GetAsync($"/mvc/runtime/{shape}?scenario={scenario}");
			using var minimal = await client.GetAsync($"/api/users/{shape}?scenario={scenario}");
			Assert.Equal(mvc.StatusCode, minimal.StatusCode);
			Assert.Equal(mvc.Content.Headers.ContentType?.ToString(), minimal.Content.Headers.ContentType?.ToString());
			Assert.Equal(await mvc.Content.ReadAsStringAsync(), await minimal.Content.ReadAsStringAsync());
		}

		using var document = JsonDocument.Parse(await client.GetStringAsync("/openapi/v1.json"));
		var paths = document.RootElement.GetProperty("paths");
		foreach (var shape in new[]
			         { "direct", "task", "value-task", "direct-value", "task-value", "value-task-value" })
		{
			var responses = paths.GetProperty($"/api/users/{shape}").GetProperty("get").GetProperty("responses");
			Assert.Equal(
				new[] { shape.EndsWith("-value") ? 200 : 204, 400, 401, 403, 404, 422 },
				responses.EnumerateObject().Select(p => int.Parse(p.Name)).Order().ToArray());
		}

		var explicitEndpoint =
			app
				.Services
				.GetRequiredService<EndpointDataSource>()
				.Endpoints
				.OfType<RouteEndpoint>()
				.Single(e => e.RoutePattern.RawText == "/api/users/explicit");
		var metadata = explicitEndpoint.Metadata.OfType<IProducesResponseTypeMetadata>().ToArray();
		Assert.All(metadata.GroupBy(m => m.StatusCode), group => Assert.Single(group));
		Assert.Equal(typeof(string), metadata.Single(m => m.StatusCode == 200).Type);
	}

	[Fact]
	public void Mvc_extension_returns_builder_and_registers_convention_once()
	{
		var services = new ServiceCollection();
		var mvc = services.AddControllers();
		Assert.Same(mvc, mvc.AddOutcomeResults());
		mvc.AddOutcomeResults();
		using var provider = services.BuildServiceProvider();
		var options = provider.GetRequiredService<IOptions<MvcOptions>>().Value;
		Assert.Single(options.Conventions.OfType<OutcomeResultProducesResponseConvention>());
	}
}
