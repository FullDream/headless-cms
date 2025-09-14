using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using Application.Abstractions;
using Application.Abstractions.IntegrationEvents.Tags;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using WebApi.Common.OpenApi;
using WebApi.Common.Results;
using WebApi.Common.Serialization;
using WebApi.Dispatchers;
using WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);


// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// auth
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
	.AddEntityFrameworkStores<AppDbContext>();


// Application
builder.Services.AddApplication();

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
app.MapIdentityApi<IdentityUser>();
app.MapHub<ContentTypesHub>("/hubs/content-types");

app.Run();