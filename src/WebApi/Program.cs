using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// Infrastructure
builder.Services.AddDbInfrastructure(builder.Configuration);


// Application
builder.Services.AddMediatR(cfg =>
	cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

builder.Services.AddControllers();
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