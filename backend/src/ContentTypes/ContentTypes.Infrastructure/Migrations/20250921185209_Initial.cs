using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentTypes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "content_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    kind = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_content_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "content_fields",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    content_type_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    label = table.Column<string>(type: "TEXT", nullable: false),
                    type = table.Column<string>(type: "TEXT", nullable: false),
                    is_required = table.Column<bool>(type: "INTEGER", nullable: false),
                    order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_content_fields", x => x.id);
                    table.ForeignKey(
                        name: "fk_content_fields_content_types_content_type_id",
                        column: x => x.content_type_id,
                        principalTable: "content_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_content_fields_content_type_id",
                table: "content_fields",
                column: "content_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_content_types_name",
                table: "content_types",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "content_fields");

            migrationBuilder.DropTable(
                name: "content_types");
        }
    }
}
