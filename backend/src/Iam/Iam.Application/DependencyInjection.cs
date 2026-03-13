using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Iam.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddIamApplication(this IServiceCollection services)
	{
		var assembly = Assembly.GetExecutingAssembly();

		services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
		services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(assembly));

		return services;
	}
}