using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManager.Migrations
{
    /// <inheritdoc />
    public partial class LandCertificateUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Surface",
                table: "LandCertificates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Surface",
                table: "LandCertificates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
