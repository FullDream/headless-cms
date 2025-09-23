using System.Reflection;
using ContentTypes.Application.Integration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using IDomainEventPublisher = Application.Abstractions.IntegrationEvents.IDomainEventPublisher;

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