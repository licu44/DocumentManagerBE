using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userGeneratedDocs_GenerateDocTypes_TypeId",
                table: "userGeneratedDocs");

            migrationBuilder.DropForeignKey(
                name: "FK_userGeneratedDocs_Users_UserId",
                table: "userGeneratedDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userGeneratedDocs",
                table: "userGeneratedDocs");

            migrationBuilder.RenameTable(
                name: "userGeneratedDocs",
                newName: "UserGeneratedDocs");

            migrationBuilder.RenameIndex(
                name: "IX_userGeneratedDocs_TypeId",
                table: "UserGeneratedDocs",
                newName: "IX_UserGeneratedDocs_TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGeneratedDocs",
                table: "UserGeneratedDocs",
                columns: new[] { "UserId", "TypeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserGeneratedDocs_GenerateDocTypes_TypeId",
                table: "UserGeneratedDocs",
                column: "TypeId",
                principalTable: "GenerateDocTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGeneratedDocs_Users_UserId",
                table: "UserGeneratedDocs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGeneratedDocs_GenerateDocTypes_TypeId",
                table: "UserGeneratedDocs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGeneratedDocs_Users_UserId",
                table: "UserGeneratedDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGeneratedDocs",
                table: "UserGeneratedDocs");

            migrationBuilder.RenameTable(
                name: "UserGeneratedDocs",
                newName: "userGeneratedDocs");

            migrationBuilder.RenameIndex(
                name: "IX_UserGeneratedDocs_TypeId",
                table: "userGeneratedDocs",
                newName: "IX_userGeneratedDocs_TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userGeneratedDocs",
                table: "userGeneratedDocs",
                columns: new[] { "UserId", "TypeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_userGeneratedDocs_GenerateDocTypes_TypeId",
                table: "userGeneratedDocs",
                column: "TypeId",
                principalTable: "GenerateDocTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userGeneratedDocs_Users_UserId",
                table: "userGeneratedDocs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
