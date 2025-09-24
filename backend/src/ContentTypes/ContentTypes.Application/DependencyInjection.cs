using System.Reflection;
using BuildingBlocks.Events;
using ContentTypes.Application.Integration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ContentTypes.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddContentTypesApplication(this IServiceCollection services)
	{
		var assembly = Assembly.GetExecutingAssembly();

		services.AddValidatorsFromAssembly(assembly);
		services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(assembly));

		services.AddScoped<IDomainEventPublisher, ContentTypeMediatorDomainEventPublisher>();
		services.AddScoped<IDomainEventPublisher, ContentTypeDomainEventPublisher>();

		return services;
	}
}