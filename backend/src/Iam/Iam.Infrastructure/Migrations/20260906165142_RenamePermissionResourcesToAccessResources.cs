using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamePermissionResourcesToAccessResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_iam_permission_resources_iam_permission_resources_parent_id",
                table: "iam_permission_resources");

            migrationBuilder.DropForeignKey(
                name: "fk_iam_resource_capabilities_iam_permission_resources_resource_id",
                table: "iam_resource_capabilities");

            migrationBuilder.DropForeignKey(
                name: "fk_iam_role_permissions_iam_permission_resources_resource_id",
                table: "iam_role_permissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_iam_permission_resources",
                table: "iam_permission_resources");

            migrationBuilder.RenameTable(
                name: "iam_permission_resources",
                newName: "iam_access_resources");

            migrationBuilder.RenameIndex(
                name: "ix_iam_permission_resources_parent_id_key",
                table: "iam_access_resources",
                newName: "ix_iam_access_resources_parent_id_key");

            migrationBuilder.AddPrimaryKey(
                name: "pk_iam_access_resources",
                table: "iam_access_resources",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_iam_access_resources_iam_access_resources_parent_id",
                table: "iam_access_resources",
                column: "parent_id",
                principalTable: "iam_access_resources",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_iam_resource_capabilities_iam_access_resources_resource_id",
                table: "iam_resource_capabilities",
                column: "resource_id",
                principalTable: "iam_access_resources",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_iam_role_permissions_iam_access_resources_resource_id",
                table: "iam_role_permissions",
                column: "resource_id",
                principalTable: "iam_access_resources",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_iam_access_resources_iam_access_resources_parent_id",
                table: "iam_access_resources");

            migrationBuilder.DropForeignKey(
                name: "fk_iam_resource_capabilities_iam_access_resources_resource_id",
                table: "iam_resource_capabilities");

            migrationBuilder.DropForeignKey(
                name: "fk_iam_role_permissions_iam_access_resources_resource_id",
                table: "iam_role_permissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_iam_access_resources",
                table: "iam_access_resources");

            migrationBuilder.RenameTable(
                name: "iam_access_resources",
                newName: "iam_permission_resources");

            migrationBuilder.RenameIndex(
                name: "ix_iam_access_resources_parent_id_key",
                table: "iam_permission_resources",
                newName: "ix_iam_permission_resources_parent_id_key");

            migrationBuilder.AddPrimaryKey(
                name: "pk_iam_permission_resources",
                table: "iam_permission_resources",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_iam_permission_resources_iam_permission_resources_parent_id",
                table: "iam_permission_resources",
                column: "parent_id",
                principalTable: "iam_permission_resources",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_iam_resource_capabilities_iam_permission_resources_resource_id",
                table: "iam_resource_capabilities",
                column: "resource_id",
                principalTable: "iam_permission_resources",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_iam_role_permissions_iam_permission_resources_resource_id",
                table: "iam_role_permissions",
                column: "resource_id",
                principalTable: "iam_permission_resources",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
