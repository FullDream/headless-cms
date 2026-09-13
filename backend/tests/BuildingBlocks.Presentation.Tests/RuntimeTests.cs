using System.Text;
using System.Text.Json;
using BuildingBlocks.Presentation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SharedKernel.Result;

namespace BuildingBlocks.Presentation.Tests;

public sealed class RuntimeTests(OutcomeTestHost host) : IClassFixture<OutcomeTestHost>
{
	public static IEnumerable<object[]> RuntimeCases()
	{
		foreach (var shape in new[]
			         { "direct", "task", "value-task", "direct-value", "task-value", "value-task-value" })
		{
			yield return [shape, "success", shape.EndsWith("-value") ? 200 : 204];
			foreach (var (scenario, status) in new[]
			         {
				         ("Failure", 400), ("Unauthenticated", 401), ("Forbidden", 403), ("NotFound", 404),
				         ("Conflict", 409), ("Validation", 422), ("BusinessRule", 422), ("mixed", 401)
			         })
				yield return [shape, scenario, status];
		}
	}

	[Theory]
	[MemberData(nameof(RuntimeCases))]
	public async Task Mvc_and_minimal_preserve_status_content_type_and_exact_body(
		string shape,
		string scenario,
		int status)
	{
		using var mvc = await host.Client.GetAsync($"/mvc/runtime/{shape}?scenario={scenario}");
		using var minimal = await host.Client.GetAsync($"/minimal/runtime/{shape}?scenario={scenario}");
		Assert.Equal(status, (int)mvc.StatusCode);
		Assert.Equal(mvc.StatusCode, minimal.StatusCode);
		Assert.Equal(mvc.Content.Headers.ContentType?.ToString(), minimal.Content.Headers.ContentType?.ToString());
		var body = await mvc.Content.ReadAsStringAsync();
		Assert.Equal(body, await minimal.Content.ReadAsStringAsync());
		if (status == 204)
		{
			Assert.Empty(body);
			Assert.Null(mvc.Content.Headers.ContentType);
			return;
		}

		if (status == 200)
		{
			Assert.Equal("application/json; charset=utf-8", mvc.Content.Headers.ContentType!.ToString());
			Assert.Equal("{\"name\":\"Ada\",\"state\":\"active\",\"attributes\":{\"displayName\":\"Ada\"}}", body);
			return;
		}

		Assert.Equal("application/problem+json; charset=utf-8", mvc.Content.Headers.ContentType!.ToString());
		using var json = JsonDocument.Parse(body);
		var root = json.RootElement;
		Assert.Equal(status, root.GetProperty("status").GetInt32());
		Assert.Equal("outcome-contract-test", root.GetProperty("traceId").GetString());
		Assert.Equal(
			status switch
			{
				400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
				401 => "https://tools.ietf.org/html/rfc9110#section-15.5.2",
				403 => "https://tools.ietf.org/html/rfc9110#section-15.5.4",
				404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
				409 => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
				422 => "https://tools.ietf.org/html/rfc4918#section-11.2",
				_ => throw new InvalidOperationException()
			},
			root.GetProperty("type").GetString());
		Assert.False(root.TryGetProperty("instance", out _));
		Assert.Equal(
			scenario == "Validation"
				? "One or more validation errors occurred."
				: status switch
				{
					400 => "Bad Request", 401 => "Unauthorized", 403 => "Forbidden", 404 => "Not Found",
					409 => "Conflict", 422 => "Unprocessable Entity", _ => throw new InvalidOperationException()
				},
			root.GetProperty("title").GetString());
		if (scenario == "Validation")
			Assert.False(root.TryGetProperty("detail", out _));
		else
			Assert.Equal("First message", root.GetProperty("detail").GetString());

		if (scenario is "Validation" or "Conflict")
		{
			var errors = root.GetProperty("errors");
			Assert.Equal(
				new[] { "First message", "Second message" },
				errors.GetProperty("name").EnumerateArray().Select(e => e.GetString()));
			Assert.Equal("Global message", errors.GetProperty("")[0].GetString());
		}
		else
			Assert.False(root.TryGetProperty("errors", out _));
	}

	[Fact]
	public async Task Minimal_only_host_works_without_AddControllers_or_AddProblemDetails()
	{
		var builder = WebApplication.CreateBuilder();
		builder.WebHost.UseTestServer();
		builder.Logging.ClearProviders();
		await using var app = builder.Build();
		app.MapGroup("/api").WithOutcomeResults().MapGet("/users", Handlers.DirectValue);
		await app.StartAsync();
		using var client = app.GetTestClient();
		using var success = await client.GetAsync("/api/users");
		Assert.Equal(200, (int)success.StatusCode);
		using var error = await client.GetAsync("/api/users?scenario=Validation");
		Assert.Equal(422, (int)error.StatusCode);
		using var json = JsonDocument.Parse(await error.Content.ReadAsStringAsync());
		Assert.True(json.RootElement.TryGetProperty("traceId", out _));
		Assert.True(json.RootElement.GetProperty("errors").TryGetProperty("Name", out _));
	}

	[Fact]
	public async Task Every_error_priority_pair_is_preserved_in_both_pipelines()
	{
		var priority = new[]
		{
			ErrorType.Unauthenticated, ErrorType.Forbidden, ErrorType.NotFound, ErrorType.Conflict,
			ErrorType.BusinessRule, ErrorType.Validation, ErrorType.Failure
		};
		var statuses = new[] { 401, 403, 404, 409, 422, 422, 400 };
		for (var high = 0; high < priority.Length; high++)
		for (var low = high + 1; low < priority.Length; low++)
		{
			var result = Result.Failure(
				new Error("low", "Low priority first", Type: priority[low]),
				new Error("high", "High priority second", Type: priority[high]));
			var mvc = await Execute(result, minimal: false);
			var minimal = await Execute(result, minimal: true);
			Assert.Equal(statuses[high], mvc.Status);
			Assert.Equal(mvc, minimal);
		}
	}

	private async Task<(int Status, string? ContentType, string Body)> Execute(Result result, bool minimal)
	{
		using var scope = host.App.Services.CreateScope();
		var context = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
		await using var stream = new MemoryStream();
		context.Response.Body = stream;
		var outcome = new OutcomeResult(result);
		if (minimal)
			await outcome.ExecuteAsync(context);
		else
			await outcome.ExecuteResultAsync(new ActionContext(context, new RouteData(), new ActionDescriptor()));
		return (context.Response.StatusCode, context.Response.ContentType, Encoding.UTF8.GetString(stream.ToArray()));
	}
}
