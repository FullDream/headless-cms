using System.Text.Json;
using System.Text.Json.Serialization;
using BuildingBlocks.Presentation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedKernel.Result;

namespace BuildingBlocks.Presentation.Tests;

public sealed class OutcomeTestHost : IAsyncLifetime
{
	public WebApplication App { get; private set; } = null!;
	public HttpClient Client { get; private set; } = null!;

	public async Task InitializeAsync()
	{
		var builder = WebApplication.CreateBuilder();
		builder.WebHost.UseTestServer();
		builder.Logging.ClearProviders();
		builder
			.Services
			.AddControllers()
			.AddOutcomeResults()
			.AddOutcomeResults()
			.AddApplicationPart(typeof(RuntimeController).Assembly)
			.AddJsonOptions(o =>
			{
				o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
				o.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
			});
		// This is the real customization hook used by DefaultProblemDetailsFactory.
		// A deterministic trace value allows exact body comparisons across requests.
		builder.Services.AddProblemDetails(o => o.CustomizeProblemDetails = context =>
			context.ProblemDetails.Extensions["traceId"] = "outcome-contract-test");
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddOpenApi();
		App = builder.Build();
		App.MapControllers(); // Existing MVC registration needs no new group convention.
		var group = App.MapGroup("/minimal").WithOutcomeResults();
		group.MapGet("/runtime/direct", Handlers.Direct);
		group.MapGet("/runtime/task", Handlers.TaskResult);
		group.MapGet("/runtime/value-task", Handlers.ValueTaskResult);
		group.MapGet("/runtime/direct-value", Handlers.DirectValue);
		group.MapGet("/runtime/task-value", Handlers.TaskValue);
		group.MapGet("/runtime/value-task-value", Handlers.ValueTaskValue);

		var metadata = group.MapGroup("/metadata");
		metadata.MapGet("/{id:guid}", Handlers.TaskValue);
		metadata.MapGet("/collection", Handlers.Collection);
		metadata.MapPost("/", Handlers.DirectValue);
		metadata.MapPost("/{id:guid}", Handlers.ValueTaskValue);
		metadata.MapDelete("/{id:guid}", Handlers.TaskResult);
		metadata.MapGet("/explicit", Handlers.DirectValue).Produces<string>(200, "text/plain").ProducesProblem(404);
		metadata.MapGet("/attribute", Handlers.WithAttribute);
		metadata
			.MapGet("/finally", Handlers.DirectValue)
			.Finally(endpoint =>
				endpoint.Metadata.Add(new ProducesResponseTypeAttribute(typeof(string), 200, "text/plain")));
		metadata.MapGroup("/parent/{parentId:guid}").MapGet("/collection", Handlers.Collection);
		metadata
			.MapGroup("/group-explicit")
			.WithMetadata(new ProducesResponseTypeAttribute(typeof(string), 200))
			.MapGet("/", Handlers.DirectValue);
		metadata.MapGroup("/repeated").WithOutcomeResults().WithOutcomeResults().MapGet("/", Handlers.DirectValue);
		metadata.MapGet("/ordinary", () => TypedResults.Ok("ordinary"));
		App.MapOpenApi();
		await App.StartAsync();
		Client = App.GetTestClient();
	}

	public async Task DisposeAsync()
	{
		Client.Dispose();
		await App.DisposeAsync();
	}

	public RouteEndpoint Endpoint(string pattern) =>
		App
			.Services
			.GetRequiredService<EndpointDataSource>()
			.Endpoints
			.OfType<RouteEndpoint>()
			.Single(endpoint => endpoint.RoutePattern.RawText == pattern);
}

public enum State
{
	Active
}

public sealed record Payload(string Name, State State, Dictionary<string, string> Attributes);

internal static class Samples
{
	internal static readonly Payload Value = new("Ada", State.Active, new() { ["DisplayName"] = "Ada" });
	internal static readonly Result Success = Result.Success();
	internal static readonly Result<Payload> SuccessValue = Result<Payload>.Success(Value);

	internal static Error[] Errors(string scenario)
	{
		if (scenario == "mixed")
			return
			[
				new("validation", "First message", "Name", ErrorType.Validation),
				new("auth", "Second message", Type: ErrorType.Unauthenticated)
			];
		var type = Enum.Parse<ErrorType>(scenario);
		return
		[
			new("sample", "First message", "Name", type), new("second", "Second message", "Name", type),
			new("global", "Global message", Type: type)
		];
	}

	internal static Result Get(string scenario) => scenario == "success" ? Success : Result.Failure(Errors(scenario));

	internal static Result<Payload> GetValue(string scenario) =>
		scenario == "success" ? SuccessValue : Result<Payload>.Failure(Errors(scenario));
}

internal static class Handlers
{
	internal static OutcomeResult Direct(string scenario = "success") => Samples.Get(scenario);
	internal static Task<OutcomeResult> TaskResult(string scenario = "success") => Task.FromResult(Direct(scenario));
	internal static ValueTask<OutcomeResult> ValueTaskResult(string scenario = "success") => new(Direct(scenario));
	internal static OutcomeResult<Payload> DirectValue(string scenario = "success") => Samples.GetValue(scenario);

	internal static Task<OutcomeResult<Payload>> TaskValue(string scenario = "success") =>
		Task.FromResult(DirectValue(scenario));

	internal static ValueTask<OutcomeResult<Payload>> ValueTaskValue(string scenario = "success") =>
		new(DirectValue(scenario));

	internal static ValueTask<OutcomeResult<IReadOnlyCollection<Payload>>> Collection() =>
		new(
			new OutcomeResult<IReadOnlyCollection<Payload>>(
				Result<IReadOnlyCollection<Payload>>.Success([Samples.Value])));

	[ProducesResponseType(typeof(string), 200, "text/plain")]
	internal static OutcomeResult<Payload> WithAttribute() => Samples.SuccessValue;
}

[ApiController]
[Route("mvc/runtime")]
public sealed class RuntimeController : ControllerBase
{
	[HttpGet("direct")]
	public OutcomeResult Direct(string scenario = "success") => Samples.Get(scenario);

	[HttpGet("task")]
	public Task<OutcomeResult> TaskResult(string scenario = "success") =>
		Task.FromResult<OutcomeResult>(Samples.Get(scenario));

	[HttpGet("value-task")]
	public ValueTask<OutcomeResult> ValueTaskResult(string scenario = "success") =>
		new(new OutcomeResult(Samples.Get(scenario)));

	[HttpGet("direct-value")]
	public OutcomeResult<Payload> DirectValue(string scenario = "success") => Samples.GetValue(scenario);

	[HttpGet("task-value")]
	public Task<OutcomeResult<Payload>> TaskValue(string scenario = "success") =>
		Task.FromResult<OutcomeResult<Payload>>(Samples.GetValue(scenario));

	[HttpGet("value-task-value")]
	public ValueTask<OutcomeResult<Payload>> ValueTaskValue(string scenario = "success") =>
		new(new OutcomeResult<Payload>(Samples.GetValue(scenario)));
}

[ApiController]
[Route("mvc/metadata")]
public sealed class MetadataController : ControllerBase
{
	[HttpGet("{id:guid}")]
	public Task<OutcomeResult<Payload>> ById(Guid id) => Handlers.TaskValue();

	[HttpGet("collection")]
	public ValueTask<OutcomeResult<IReadOnlyCollection<Payload>>> Collection() => Handlers.Collection();

	[HttpPost]
	public OutcomeResult<Payload> Create() => Samples.SuccessValue;

	[HttpPost("{id:guid}")]
	public ValueTask<OutcomeResult<Payload>> CreateById(Guid id) => Handlers.ValueTaskValue();

	[HttpDelete("{id:guid}")]
	public Task<OutcomeResult> Delete(Guid id) => Handlers.TaskResult();

	[HttpGet("explicit")]
	[ProducesResponseType(typeof(string), 200, "text/plain")]
	[ProducesResponseType(typeof(ProblemDetails), 404, "application/problem+json")]
	public OutcomeResult<Payload> Explicit() => Samples.SuccessValue;
}
