using System.Data;
using ContentEntries.Application.Abstractions;
using Contracts;
using Dapper;

namespace ContentEntries.Infrastructure.Schema;

public class SqlContentSchemaManager(IDbConnection db, ISchemaSqlGenerator sql) : IContentSchemaManager
{
	public Task EnsureStructureCreatedAsync(ContentTypeSchemaSnapshot schema, CancellationToken ct = default)
	{
		var sqlString = sql.GenerateCreateTableSql(schema);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}

	public Task RenameStructureAsync(string oldName, string newName, CancellationToken ct = default)
	{
		var sqlString = sql.GenerateRenameTableSql(oldName, newName);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}


	public Task RemoveStructureAsync(string contentTypeName, CancellationToken ct = default)
	{
		var sqlString = sql.GenerateDropTableSql(contentTypeName);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}

	public Task AddFieldToStructureAsync(ContentTypeSchemaSnapshot schema, ContentFieldDef field,
		CancellationToken ct = default)
	{
		var sqlString = sql.GenerateAddColumnSql(schema, field);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}

	public Task RemoveFieldFromStructureAsync(ContentTypeSchemaSnapshot schema, ContentFieldDef field,
		CancellationToken ct = default)
	{
		var sqlString = sql.GenerateDropColumnSql(schema, field);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}

	public Task RenameFieldInStructureAsync(ContentTypeSchemaSnapshot schema, string oldFieldName, string newFieldName,
		CancellationToken ct = default)
	{
		var sqlString = sql.GenerateRenameColumnSql(schema, oldFieldName, newFieldName);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}

	public Task ChangeFieldTypeInStructureAsync(ContentTypeSchemaSnapshot schema, ContentFieldDef field,
		CancellationToken ct = default)
	{
		return Task.CompletedTask;
	}
}