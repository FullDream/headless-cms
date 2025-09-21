using Application.Abstractions;
using ContentEntries.Infrastructure.Common.Naming;
using Humanizer;
using SharedKernel;

namespace ContentEntries.Infrastructure.Schema;

internal class SqliteSchemaSqlGenerator(IStorageNameResolver nameResolver) : ISchemaSqlGenerator
{
	public string GenerateCreateTableSql(ContentFieldsSnapshot schema)
	{
		var fields = schema.Fields
			.Select(f =>
				$"[{f.Value.Name.Underscore()}] {MapFieldType(f.Value.Type)} {(f.Value.IsRequired ? "NOT NULL" : "NULL")}")
			.ToList();

		fields.Insert(0, "[id] TEXT PRIMARY KEY");
		fields.Add("[created_at] DATETIME NOT NULL");
		fields.Add("[updated_at] DATETIME NOT NULL");

		return $"""
		            CREATE TABLE [{GetTableName(schema.ContentTypeName)}] (
		                {string.Join(",\n    ", fields)}
		            );
		        """;
	}

	public string GenerateRenameTableSql(string oldName, string newName) =>
		$"ALTER TABLE [{GetTableName(oldName)}] RENAME TO [{GetTableName(newName)}];";

	public string GenerateAddColumnSql(ContentFieldsSnapshot schema, ContentFieldDef field)
		=>
			$"ALTER TABLE [{GetTableName(schema.ContentTypeName)}] ADD COLUMN [{field.Name.Underscore()}] {MapFieldType(field.Type)};";

	public string GenerateDropColumnSql(ContentFieldsSnapshot schema, ContentFieldDef removedField)
	{
		var quotedNames = schema.Fields
			.Select(f => $"[{f.Value.Name.Underscore()}]")
			.ToList();

		quotedNames.Add("id");
		quotedNames.Add("created_at");
		quotedNames.Add("updated_at");

		var originalTable = GetTableName(schema.ContentTypeName);
		var tempTable = $"{originalTable}_temp";

		var createTempTableSql = GenerateCreateTableSql(schema)
			.Replace($"[{originalTable}]", $"[{tempTable}]");

		return $"""
		            {createTempTableSql}

		            INSERT INTO [{tempTable}] ({string.Join(", ", quotedNames)})
		            SELECT {string.Join(", ", quotedNames)} FROM [{originalTable}];

		            DROP TABLE [{originalTable}];

		            ALTER TABLE [{tempTable}] RENAME TO [{originalTable}];
		        """;
	}

	public string GenerateRenameColumnSql(ContentFieldsSnapshot schema, string oldName, string newName)
		=>
			$"ALTER TABLE [{GetTableName(schema.ContentTypeName)}] RENAME COLUMN [{oldName.Underscore()}] TO [{newName.Underscore()}];";

	public string GenerateDropTableSql(ContentFieldsSnapshot schema)
		=> $"DROP TABLE IF EXISTS [{GetTableName(schema.ContentTypeName)}];";

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