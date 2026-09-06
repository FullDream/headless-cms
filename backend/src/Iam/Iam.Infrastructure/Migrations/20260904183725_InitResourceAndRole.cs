using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitResourceAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "iam_permission_resources",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    key = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    parent_id = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_iam_permission_resources", x => x.id);
                    table.ForeignKey(
                        name: "fk_iam_permission_resources_iam_permission_resources_parent_id",
                        column: x => x.parent_id,
                        principalTable: "iam_permission_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "iam_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_iam_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "iam_resource_capabilities",
                columns: table => new
                {
                    action = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    resource_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    available_scopes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_iam_resource_capabilities", x => new { x.resource_id, x.action });
                    table.ForeignKey(
                        name: "fk_iam_resource_capabilities_iam_permission_resources_resource_id",
                        column: x => x.resource_id,
                        principalTable: "iam_permission_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "iam_role_permissions",
                columns: table => new
                {
                    resource_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    action = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    role_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    scope = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_iam_role_permissions", x => new { x.role_id, x.resource_id, x.action });
                    table.ForeignKey(
                        name: "fk_iam_role_permissions_iam_permission_resources_resource_id",
                        column: x => x.resource_id,
                        principalTable: "iam_permission_resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_iam_role_permissions_iam_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "iam_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_iam_permission_resources_parent_id_key",
                table: "iam_permission_resources",
                columns: new[] { "parent_id", "key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_iam_role_permissions_resource_id",
                table: "iam_role_permissions",
                column: "resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_iam_roles_name",
                table: "iam_roles",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "iam_resource_capabilities");

            migrationBuilder.DropTable(
                name: "iam_role_permissions");

            migrationBuilder.DropTable(
                name: "iam_permission_resources");

            migrationBuilder.DropTable(
                name: "iam_roles");
        }
    }
}
