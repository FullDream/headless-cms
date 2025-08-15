using Core.ContentTypes;
using Humanizer;
using Infrastructure.Common.Naming;

namespace Infrastructure.ContentTypes.Schema;

internal class SqliteSchemaSqlGenerator(IStorageNameResolver nameResolver) : ISchemaSqlGenerator
{
	public string GenerateCreateTableSql(ContentType contentType)
	{
		var fields = contentType.Fields
			.Select(f => $"[{f.Name.Underscore()}] {MapFieldType(f.Type)} {(f.IsRequired ? "NOT NULL" : "NULL")}")
			.ToList();

		fields.Insert(0, "[id] TEXT PRIMARY KEY");
		fields.Add("[created_at] DATETIME NOT NULL");
		fields.Add("[updated_at] DATETIME NOT NULL");

		return $"""
		            CREATE TABLE [{GetTableName(contentType.Name)}] (
		                {string.Join(",\n    ", fields)}
		            );
		        """;
	}

	public string GenerateRenameTableSql(string oldName, string newName) =>
		$"ALTER TABLE [{GetTableName(oldName)}] RENAME TO [{GetTableName(newName)}];";

	public string GenerateAddColumnSql(ContentType contentType, ContentField field)
		=>
			$"ALTER TABLE [{GetTableName(contentType.Name)}] ADD COLUMN [{field.Name.Underscore()}] {MapFieldType(field.Type)};";

	public string GenerateDropColumnSql(ContentType contentType, ContentField removedField)
	{
		var quotedNames = contentType.Fields
			.Select(f => $"[{f.Name.Underscore()}]")
			.ToList();


		var originalTable = GetTableName(contentType.Name);
		var tempTable = $"{originalTable}_temp";

		var createTempTableSql = GenerateCreateTableSql(contentType)
			.Replace($"[{originalTable}]", $"[{tempTable}]");

		return $"""
		            BEGIN TRANSACTION;
		        
		            {createTempTableSql}
		        
		            INSERT INTO [{tempTable}] ({string.Join(", ", quotedNames)})
		            SELECT {string.Join(", ", quotedNames)} FROM [{originalTable}];
		        
		            DROP TABLE [{originalTable}];
		        
		            ALTER TABLE [{tempTable}] RENAME TO [{originalTable}];
		        
		            COMMIT;
		        """;
	}

	public string GenerateRenameColumnSql(ContentType contentType, string oldName, string newName)
		=>
			$"ALTER TABLE [{GetTableName(contentType.Name)}] RENAME COLUMN [{oldName.Underscore()}] TO [{newName.Underscore()}];";

	public string GenerateDropTableSql(ContentType contentType)
		=> $"DROP TABLE IF EXISTS [{GetTableName(contentType.Name)}];";

	public string MapFieldType(FieldType type)
		=> type switch
		{
			FieldType.Integer => "INTEGER",
			FieldType.Decimal => "REAL",
			FieldType.ShortText or FieldType.LongText => "TEXT",
			FieldType.Boolean => "INTEGER",
			_ => throw new NotSupportedException($"Unsupported type: {type}")
		};

	private string GetTableName(string contentTypeName) => nameResolver.Resolve(contentTypeName);
}