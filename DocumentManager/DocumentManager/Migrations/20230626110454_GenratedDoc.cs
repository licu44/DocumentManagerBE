using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManager.Migrations
{
    /// <inheritdoc />
    public partial class GenratedDoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "IdCards");

            migrationBuilder.CreateTable(
                name: "GenerateDocTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenerateDocTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "userGeneratedDocs",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WordDocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userGeneratedDocs", x => new { x.UserId, x.TypeId });
                    table.ForeignKey(
                        name: "FK_userGeneratedDocs_GenerateDocTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "GenerateDocTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userGeneratedDocs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userGeneratedDocs_TypeId",
                table: "userGeneratedDocs",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userGeneratedDocs");

            migrationBuilder.DropTable(
                name: "GenerateDocTypes");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "IdCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
