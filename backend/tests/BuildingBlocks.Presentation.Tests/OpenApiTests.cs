using System.Text.Json;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Presentation.Tests;

public sealed class OpenApiTests(OutcomeTestHost host) : IClassFixture<OutcomeTestHost>
{
	public static IEnumerable<object[]> Operations()
	{
		yield return ["/{id}", "get", new[] { 200, 400, 401, 403, 404, 422 }];
		yield return ["/collection", "get", new[] { 200, 400, 401, 403, 422 }];
		yield return ["", "post", new[] { 200, 400, 401, 403, 409, 422 }];
		yield return ["/{id}", "post", new[] { 200, 400, 401, 403, 404, 409, 422 }];
		yield return ["/{id}", "delete", new[] { 204, 400, 401, 403, 404 }];
		yield return ["/explicit", "get", new[] { 200, 400, 401, 403, 404, 422 }];
	}

	[Theory]
	[MemberData(nameof(Operations))]
	public async Task Actual_OpenApi_preserves_Mvc_and_automatically_describes_Minimal(
		string suffix,
		string method,
		int[] expectedStatuses)
	{
		using var document = JsonDocument.Parse(await host.Client.GetStringAsync("/openapi/v1.json"));
		var paths = document.RootElement.GetProperty("paths");
		var mvc = Responses(paths, "/mvc/metadata" + suffix, method);
		var minimal = Responses(paths, "/minimal/metadata" + suffix, method);
		Assert.Equal(expectedStatuses, Statuses(mvc));
		Assert.Equal(expectedStatuses, Statuses(minimal));
		foreach (var status in expectedStatuses)
		{
			var mvcResponse = mvc.GetProperty(status.ToString());
			var minimalResponse = minimal.GetProperty(status.ToString());
			if (status == 204)
			{
				Assert.False(mvcResponse.TryGetProperty("content", out _));
				Assert.False(minimalResponse.TryGetProperty("content", out _));
				continue;
			}

			var contentType = status >= 400 ? "application/problem+json" :
				suffix == "/explicit" ? "text/plain" : "application/json";
			var mvcSchema = mvcResponse.GetProperty("content").GetProperty(contentType).GetProperty("schema");
			var minimalSchema = minimalResponse.GetProperty("content").GetProperty(contentType).GetProperty("schema");
			Assert.Equal(mvcSchema.GetRawText(), minimalSchema.GetRawText());
			if (status >= 400)
				Assert.EndsWith(
					status is 400 or 409 or 422 ? "/ValidationProblemDetails" : "/ProblemDetails",
					mvcSchema.GetProperty("$ref").GetString());
			else if (suffix == "/collection")
			{
				Assert.Equal("array", mvcSchema.GetProperty("type").GetString());
				Assert.EndsWith("/Payload", mvcSchema.GetProperty("items").GetProperty("$ref").GetString());
			}
			else if (suffix != "/explicit")
				Assert.EndsWith("/Payload", mvcSchema.GetProperty("$ref").GetString());
		}
	}

	[Theory]
	[InlineData("direct", 204)]
	[InlineData("task", 204)]
	[InlineData("value-task", 204)]
	[InlineData("direct-value", 200)]
	[InlineData("task-value", 200)]
	[InlineData("value-task-value", 200)]
	public async Task All_six_return_shapes_have_automatic_metadata(string shape, int successStatus)
	{
		using var document = JsonDocument.Parse(await host.Client.GetStringAsync("/openapi/v1.json"));
		foreach (var pipeline in new[] { "mvc", "minimal" })
		{
			var responses = Responses(document.RootElement.GetProperty("paths"), $"/{pipeline}/runtime/{shape}", "get");
			Assert.Equal(new[] { successStatus, 400, 401, 403, 404, 422 }, Statuses(responses));
		}
	}

	[Theory]
	[InlineData("/minimal/metadata/explicit")]
	[InlineData("/minimal/metadata/attribute")]
	[InlineData("/minimal/metadata/finally")]
	[InlineData("/minimal/metadata/group-explicit/")]
	public void Explicit_metadata_is_preserved_without_duplicate_statuses(string pattern)
	{
		var endpoint = host.Endpoint(pattern);
		var responses = endpoint
			.Metadata
			.Select(item => item switch
			{
				IProducesResponseTypeMetadata produces => (produces.StatusCode, produces.Type),
				ProducesResponseTypeAttribute produces => (produces.StatusCode, produces.Type),
				_ => (0, (Type?)null)
			})
			.Where(response => response.Item1 != 0)
			.ToArray();
		Assert.All(responses.GroupBy(r => r.Item1), group => Assert.Single(group));
		Assert.Equal(typeof(string), responses.Single(r => r.Item1 == 200).Item2);
	}

	[Fact]
	public void Mvc_explicit_metadata_is_not_duplicated()
	{
		var descriptions = host
			.App
			.Services
			.GetRequiredService<IApiDescriptionGroupCollectionProvider>()
			.ApiDescriptionGroups
			.Items
			.SelectMany(group => group.Items);
		var action = descriptions.Single(d => d.RelativePath == "mvc/metadata/explicit");
		var filters = ((ControllerActionDescriptor)action.ActionDescriptor)
			.FilterDescriptors
			.Select(f => f.Filter)
			.OfType<ProducesResponseTypeAttribute>()
			.ToArray();
		Assert.All(filters.GroupBy(f => f.StatusCode), group => Assert.Single(group));
		Assert.Equal(typeof(string), filters.Single(f => f.StatusCode == 200).Type);
	}

	[Fact]
	public void Parent_route_parameters_prevent_collection_404_suppression()
	{
		var responses = host
			.Endpoint("/minimal/metadata/parent/{parentId:guid}/collection")
			.Metadata
			.OfType<IProducesResponseTypeMetadata>()
			.ToArray();
		Assert.Contains(responses, r => r.StatusCode == 404);
		Assert.DoesNotContain(responses, r => r.StatusCode == 409);
	}

	[Fact]
	public void Repeated_group_conventions_are_idempotent_and_other_return_types_are_untouched()
	{
		var responses = host.Endpoint("/minimal/metadata/repeated/").Metadata.OfType<IProducesResponseTypeMetadata>();
		Assert.All(responses.GroupBy(r => r.StatusCode), group => Assert.Single(group));
		var ordinary = host.Endpoint("/minimal/metadata/ordinary").Metadata.OfType<IProducesResponseTypeMetadata>();
		Assert.Equal(200, Assert.Single(ordinary).StatusCode);
	}

	private static JsonElement Responses(JsonElement paths, string path, string method)
	{
		if (!paths.TryGetProperty(path, out var operation))
			operation = paths.GetProperty(path + "/");
		return operation.GetProperty(method).GetProperty("responses");
	}

	private static int[] Statuses(JsonElement responses) =>
		responses.EnumerateObject().Select(property => int.Parse(property.Name)).Order().ToArray();
}
