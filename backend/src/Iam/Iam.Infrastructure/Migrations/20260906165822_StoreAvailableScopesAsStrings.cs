using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iam.Infrastructure.Migrations;

public partial class StoreAvailableScopesAsStrings : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) =>
        migrationBuilder.Sql("""
            UPDATE iam_resource_capabilities
            SET available_scopes = (
                SELECT json_group_array(
                    CASE
                        WHEN type = 'integer' AND value = 1 THEN 'Own'
                        WHEN type = 'integer' AND value = 2 THEN 'All'
                        ELSE value
                    END)
                FROM json_each(iam_resource_capabilities.available_scopes)
            );
            """);

    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.Sql("""
            UPDATE iam_resource_capabilities
            SET available_scopes = (
                SELECT json_group_array(
                    CASE
                        WHEN type = 'text' AND value = 'Own' THEN 1
                        WHEN type = 'text' AND value = 'All' THEN 2
                        ELSE value
                    END)
                FROM json_each(iam_resource_capabilities.available_scopes)
            );
            """);
}
