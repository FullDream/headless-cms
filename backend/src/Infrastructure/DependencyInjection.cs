using System.Data;
using Application.Abstractions;
using Core.ContentEntries;
using Core.ContentTypes;
using Infrastructure.Common.Configuration;
using Infrastructure.Common.Naming;
using Infrastructure.ContentEntries;
using Infrastructure.ContentTypes;
using Infrastructure.ContentTypes.Schema;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlKata.Compilers;

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
				.UseSnakeCaseNamingConvention());
		services.AddScoped<IContentTypeRepository, ContentTypeRepository>();
		services.AddScoped<IContentTypeFieldsProvider, ContentTypeFieldsProvider>();
		services.AddScoped<IContentTypeExistenceChecker, ContentTypeExistenceChecker>();

		services.AddSingleton<IContentEntryDataMapper, ContentEntryDataMapper>();
		services.AddSingleton<IStorageFieldNameConverter, StorageFieldNameConverter>();

		services.AddScoped<IDbConnection>(_ => new SqliteConnection(connectionString));
		services.AddScoped<ISchemaSqlGenerator, SqliteSchemaSqlGenerator>();
		services.AddScoped<IContentTypeSchemaManager, SqlContentTypeSchemaManager>();
		services.AddScoped<Compiler, SqliteCompiler>();

		services.AddScoped<IContentEntryRepository, ContentEntryRepository>();


		return services;
	}
}