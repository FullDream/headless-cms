using System.Data;
using Application.Abstractions;
using Core.ContentTypes;
using Infrastructure.Common.Configuration;
using Infrastructure.Common.Naming;
using Infrastructure.ContentTypes;
using Infrastructure.ContentTypes.Schema;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration config)
	{
		services.Configure<ContentStorageOptions>(config.GetSection("ContentStorage"));
		services.AddSingleton<IStorageNameResolver, SnakeCaseStorageNameResolver>();

		var connectionString = config.GetConnectionString("DefaultConnection");

		services.AddDbContext<AppDbContext>(options =>
				options.UseSqlite(connectionString)
					.UseSnakeCaseNamingConvention())
			.AddScoped<IContentTypeRepository, ContentTypeRepository>();


		services.AddScoped<IDbConnection>(_ => new SqliteConnection(connectionString));

		services.AddScoped<ISchemaSqlGenerator, SqliteSchemaSqlGenerator>();
		services.AddScoped<IContentTypeSchemaManager, SqlContentTypeSchemaManager>();

		return services;
	}
}