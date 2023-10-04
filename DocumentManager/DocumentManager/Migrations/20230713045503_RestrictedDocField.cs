using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManager.Migrations
{
    /// <inheritdoc />
    public partial class RestrictedDocField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Restricted",
                table: "GenerateDocTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Restricted",
                table: "GenerateDocTypes");
        }
    }
}
