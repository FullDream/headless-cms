using Core.ContentTypes;

namespace Infrastructure.ContentTypes.Schema;

public interface ISchemaSqlGenerator
{
	string GenerateCreateTableSql(ContentType contentType);
	string GenerateRenameTableSql(string oldName, string newName);
	string GenerateAddColumnSql(ContentType contentType, ContentField field);
	string GenerateDropColumnSql(ContentType contentType, ContentField field);
	string GenerateRenameColumnSql(ContentType contentType, string oldName, string newName);
	string GenerateDropTableSql(ContentType contentType);
	string MapFieldType(FieldType type);
}