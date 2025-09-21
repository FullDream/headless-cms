using System.Data;
using Application.Abstractions;
using ContentEntries.Core;
using ContentEntries.Infrastructure.Common.Configuration;
using ContentEntries.Infrastructure.Common.Naming;
using ContentEntries.Infrastructure.Schema;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlKata.Compilers;

namespace ContentEntries.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddContentEntriesInfrastructure(
		this IServiceCollection services,
		IConfiguration config)
	{
		services.Configure<ContentStorageOptions>(config.GetSection("ContentStorage"));

		services.AddSingleton<IStorageNameResolver, SnakeCaseStorageNameResolver>();
		services.AddSingleton<IContentEntryDataMapper, ContentEntryDataMapper>();
		services.AddSingleton<IStorageFieldNameConverter, StorageFieldNameConverter>();

		var connectionString = config.GetConnectionString("DefaultConnection");

		services.AddScoped<IDbConnection>(_ => new SqliteConnection(connectionString));
		services.AddScoped<ISchemaSqlGenerator, SqliteSchemaSqlGenerator>();
		services.AddScoped<IContentSchemaManager, SqlContentSchemaManager>();
		services.AddScoped<Compiler, SqliteCompiler>();

		services.AddScoped<IContentEntryRepository, ContentEntryRepository>();

		return services;
	}
}