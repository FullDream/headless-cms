using System.Reflection;
using ContentEntries.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ContentEntries.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddContentEntriesApplication(this IServiceCollection services)
	{
		var assembly = Assembly.GetExecutingAssembly();

		services.AddValidatorsFromAssembly(assembly);
		services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(assembly));

		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ContentEntryConversionBehavior<,>));

		return services;
	}
}