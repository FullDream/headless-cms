using Contracts;
using SharedKernel;

namespace ContentEntries.Infrastructure.Schema;

public interface ISchemaSqlGenerator
{
	string GenerateCreateTableSql(ContentTypeSchemaSnapshot schema);
	string GenerateRenameTableSql(string oldName, string newName);
	string GenerateAddColumnSql(ContentTypeSchemaSnapshot schema, ContentFieldDef field);
	string GenerateDropColumnSql(ContentTypeSchemaSnapshot schema, ContentFieldDef field);
	string GenerateRenameColumnSql(ContentTypeSchemaSnapshot schema, string oldName, string newName);
	string GenerateDropTableSql(string contentTypeName);
	string MapFieldType(FieldType type);
}