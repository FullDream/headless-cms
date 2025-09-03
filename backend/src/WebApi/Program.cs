using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using Infrastructure;
using Scalar.AspNetCore;
using WebApi.Common.OpenApi;
using WebApi.Common.Results;
using WebApi.Common.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Application
builder.Services.AddApplication();

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

app.Run();