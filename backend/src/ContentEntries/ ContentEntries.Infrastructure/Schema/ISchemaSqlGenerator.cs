using Application.Abstractions;
using SharedKernel;

namespace ContentEntries.Infrastructure.Schema;

public interface ISchemaSqlGenerator
{
	string GenerateCreateTableSql(ContentFieldsSnapshot schema);
	string GenerateRenameTableSql(string oldName, string newName);
	string GenerateAddColumnSql(ContentFieldsSnapshot schema, ContentFieldDef field);
	string GenerateDropColumnSql(ContentFieldsSnapshot schema, ContentFieldDef field);
	string GenerateRenameColumnSql(ContentFieldsSnapshot schema, string oldName, string newName);
	string GenerateDropTableSql(ContentFieldsSnapshot schema);
	string MapFieldType(FieldType type);
}