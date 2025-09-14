using System.Reflection;
using Application.Abstractions;
using Application.Abstractions.IntegrationEvents;
using Application.Common.Messaging;
using Application.Common.Validation;
using Application.ContentEntries.Behaviors;
using Application.ContentTypes.Integration;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		var assembly = Assembly.GetExecutingAssembly();

		services.AddValidatorsFromAssembly(assembly);
		services.AddMediatR(cfg =>
			cfg.RegisterServicesFromAssembly(assembly));

		services.AddSingleton<IDomainEventPublisher, MediatorDomainEventPublisher>();
		services.AddScoped<IIntegrationEventCollector, ContentTypeIntegrationEventCollector>();

		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ContentEntryConversionBehavior<,>));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		return services;
	}
}