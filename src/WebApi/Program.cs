using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// Infrastructure
builder.Services.AddDbInfrastructure(builder.Configuration);


// Application
builder.Services.AddMediatR(cfg =>
	cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));


var enumConverter = new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false);

builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(enumConverter));
builder.Services.AddControllers()
	.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(enumConverter));


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();