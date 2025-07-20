using Core.ContentTypes;
using Infrastructure.ContentTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddDbInfrastructure(
		this IServiceCollection services,
		IConfiguration config)
	{
		services.AddDbContext<AppDbContext>(options =>
				options.UseSqlite(config.GetConnectionString("DefaultConnection")))
			.AddScoped<IContentTypeRepository, ContentTypeRepository>();

		return services;
	}
}