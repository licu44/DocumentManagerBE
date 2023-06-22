using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManager.Migrations
{
    /// <inheritdoc />
    public partial class UserDocsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersDoc_DocumentTypes_TypeId",
                table: "UsersDoc");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersDoc_Users_UserId",
                table: "UsersDoc");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersDoc",
                table: "UsersDoc");

            migrationBuilder.RenameTable(
                name: "UsersDoc",
                newName: "UserDocs");

            migrationBuilder.RenameIndex(
                name: "IX_UsersDoc_TypeId",
                table: "UserDocs",
                newName: "IX_UserDocs_TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDocs",
                table: "UserDocs",
                columns: new[] { "UserId", "TypeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserDocs_DocumentTypes_TypeId",
                table: "UserDocs",
                column: "TypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDocs_Users_UserId",
                table: "UserDocs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDocs_DocumentTypes_TypeId",
                table: "UserDocs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDocs_Users_UserId",
                table: "UserDocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDocs",
                table: "UserDocs");

            migrationBuilder.RenameTable(
                name: "UserDocs",
                newName: "UsersDoc");

            migrationBuilder.RenameIndex(
                name: "IX_UserDocs_TypeId",
                table: "UsersDoc",
                newName: "IX_UsersDoc_TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersDoc",
                table: "UsersDoc",
                columns: new[] { "UserId", "TypeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersDoc_DocumentTypes_TypeId",
                table: "UsersDoc",
                column: "TypeId",
                principalTable: "DocumentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersDoc_Users_UserId",
                table: "UsersDoc",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
