using BuildingBlocks;
using ContentTypes.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContentTypes.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddContentTypesInfrastructure(
		this IServiceCollection services,
		IConfiguration config)
	{
		var connectionString = config.GetConnectionString("DefaultConnection");

		services.AddDbContext<AppDbContext>(options =>
			options.UseSqlite(connectionString)
				.UseSnakeCaseNamingConvention());
		services.AddScoped<IContentTypeRepository, ContentTypeRepository>();
		services.AddScoped<IContentTypeFieldsProvider, ContentTypeFieldsProvider>();
		services.AddScoped<IContentTypeExistenceChecker, ContentTypeExistenceChecker>();

		return services;
	}
}