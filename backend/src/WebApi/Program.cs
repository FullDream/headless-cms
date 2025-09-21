using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Abstractions;
using Application.Abstractions.IntegrationEvents.Tags;
using ContentEntries.Application;
using ContentEntries.Infrastructure;
using ContentTypes.Application;
using ContentTypes.Infrastructure;
using Scalar.AspNetCore;
using WebApi.Common.OpenApi;
using WebApi.Common.Results;
using WebApi.Common.Serialization;
using WebApi.Dispatchers;
using WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddContentTypesInfrastructure(builder.Configuration);
builder.Services.AddContentTypesApplication();

builder.Services.AddContentEntriesInfrastructure(builder.Configuration);
builder.Services.AddContentEntriesApplication();


// auth
builder.Services.AddAuthorization();

builder.Services.AddSignalR();

builder.Services.AddSingleton<IEventDispatcher<ContentTypeEventTag>, ContentTypeDispatcher>();

var enumConverter = new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false);

builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(enumConverter));
builder.Services.AddControllers(options => options.Conventions.Add(new OutcomeResultProducesResponseConvention()))
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(enumConverter);
		options.JsonSerializerOptions.DictionaryKeyPolicy = new SegmentedNamingPolicy(JsonNamingPolicy.CamelCase);
	});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(o => o.AddSchemaTransformer<FluentValidationSchemaTransformer>());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ContentTypesHub>("/hubs/content-types");

app.Run();