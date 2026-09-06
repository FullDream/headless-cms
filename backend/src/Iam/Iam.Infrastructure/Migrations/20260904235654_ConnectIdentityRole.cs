using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConnectIdentityRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_iam_role_permissions_iam_roles_role_id",
                table: "iam_role_permissions");

            migrationBuilder.DropTable(
                name: "iam_roles");

            migrationBuilder.AddForeignKey(
                name: "fk_iam_role_permissions_asp_net_roles_role_id",
                table: "iam_role_permissions",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_iam_role_permissions_asp_net_roles_role_id",
                table: "iam_role_permissions");

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

            migrationBuilder.CreateIndex(
                name: "ix_iam_roles_name",
                table: "iam_roles",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_iam_role_permissions_iam_roles_role_id",
                table: "iam_role_permissions",
                column: "role_id",
                principalTable: "iam_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
