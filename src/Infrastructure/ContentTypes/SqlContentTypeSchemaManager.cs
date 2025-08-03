using System.Data;
using Application.Abstractions;
using Core.ContentTypes;
using Dapper;
using Infrastructure.ContentTypes.Schema;

namespace Infrastructure.ContentTypes;

public class SqlContentTypeSchemaManager(IDbConnection db, ISchemaSqlGenerator sql) : IContentTypeSchemaManager
{
	public Task EnsureStructureCreatedAsync(ContentType contentType, CancellationToken ct = default)
	{
		var sqlString = sql.GenerateCreateTableSql(contentType);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}

	public Task RenameStructureAsync(string oldName, string newName, CancellationToken ct = default)
	{
		var sqlString = sql.GenerateRenameTableSql(oldName, newName);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}


	public Task RemoveStructureAsync(ContentType contentType, CancellationToken ct = default)
	{
		var sqlString = sql.GenerateDropTableSql(contentType);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}

	public Task AddFieldToStructureAsync(ContentType contentType, ContentField field, CancellationToken ct = default)
	{
		var sqlString = sql.GenerateAddColumnSql(contentType, field);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}

	public Task RemoveFieldFromStructureAsync(ContentType contentType, ContentField field,
		CancellationToken ct = default)
	{
		var sqlString = sql.GenerateDropColumnSql(contentType, field);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}

	public Task RenameFieldInStructureAsync(ContentType contentType, string oldFieldName, string newFieldName,
		CancellationToken ct = default)
	{
		var sqlString = sql.GenerateRenameColumnSql(contentType, oldFieldName, newFieldName);
		return db.ExecuteAsync(new CommandDefinition(sqlString, ct));
	}

	public Task ChangeFieldTypeInStructureAsync(ContentType contentType, ContentField field,
		CancellationToken ct = default)
	{
		return Task.CompletedTask;
	}
}