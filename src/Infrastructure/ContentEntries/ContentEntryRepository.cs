using System.Data;
using Core.ContentEntries;
using Dapper;
using Infrastructure.Common.Naming;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Infrastructure.ContentEntries;

public class ContentEntryRepository(
	IDbConnection db,
	Compiler compiler,
	IStorageNameResolver nameResolver,
	IContentEntryDataMapper mapper,
	IStorageFieldNameConverter fieldNameConverter) : IContentEntryRepository
{
	private readonly QueryFactory queryFactory = new QueryFactory(db, compiler);

	public async Task<IReadOnlyCollection<ContentEntry>> QueryAsync(string contentTypeName,
		CancellationToken ct = default)
	{
		var query = queryFactory.Query(nameResolver.Resolve(contentTypeName));
		var compiler = queryFactory.Compiler.Compile(query);

		var rows = await queryFactory.Connection
			.QueryAsync(compiler.Sql, compiler.NamedBindings);

		return rows.Select(row => mapper.Map((IDictionary<string, object?>)row)).ToList().AsReadOnly();
	}

	public Task<ContentEntry?> FindAsync(string contentTypeName, Guid id, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}

	public async Task<ContentEntry> AddAsync(string contentTypeName, ContentEntry contentEntry,
		CancellationToken ct = default)
	{
		var insertData = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
		{
			["id"] = contentEntry.Id,
			["created_at"] = contentEntry.CreatedAt,
			["updated_at"] = contentEntry.UpdatedAt
		};

		foreach (var (key, value) in contentEntry.FieldValues)
			insertData[fieldNameConverter.ToStorage(key)] = value;

		var tableName = nameResolver.Resolve(contentTypeName);
		var insert = queryFactory.Query(tableName).AsInsert(insertData);
		var insertSql = queryFactory.Compiler.Compile(insert);

		await queryFactory.Connection.ExecuteAsync(
			new CommandDefinition(insertSql.Sql, insertSql.NamedBindings, cancellationToken: ct));

		return contentEntry;
	}

	public Task UpdateAsync(string contentTypeName, ContentEntry entry, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}

	public Task DeleteAsync(string contentTypeName, Guid id, CancellationToken ct = default)
	{
		throw new NotImplementedException();
	}
}