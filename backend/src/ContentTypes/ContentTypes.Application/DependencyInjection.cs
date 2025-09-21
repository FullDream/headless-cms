using System.Reflection;
using Application.Abstractions;
using Application.Abstractions.IntegrationEvents;
using Application.Abstractions.Messaging;
using Application.Abstractions.Validation;
using ContentTypes.Application.Integration;
using FluentValidation;
using MediatR;
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

		services.AddSingleton<IDomainEventPublisher, MediatorDomainEventPublisher>();
		services.AddScoped<IIntegrationEventCollector, ContentTypeIntegrationEventCollector>();

		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		return services;
	}
}