using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManager.Migrations
{
    /// <inheritdoc />
    public partial class StatusTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorizationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EngineeringStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineeringStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FeddbackId = table.Column<int>(type: "int", nullable: false),
                    AuthorizationId = table.Column<int>(type: "int", nullable: false),
                    EngineeringId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStatuses_AuthorizationStatuses_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "AuthorizationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStatuses_EngineeringStatuses_EngineeringId",
                        column: x => x.EngineeringId,
                        principalTable: "EngineeringStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStatuses_FeedbackStatuses_FeddbackId",
                        column: x => x.FeddbackId,
                        principalTable: "FeedbackStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStatuses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStatuses_AuthorizationId",
                table: "UserStatuses",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatuses_EngineeringId",
                table: "UserStatuses",
                column: "EngineeringId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatuses_FeddbackId",
                table: "UserStatuses",
                column: "FeddbackId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStatuses_UserId",
                table: "UserStatuses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserStatuses");

            migrationBuilder.DropTable(
                name: "AuthorizationStatuses");

            migrationBuilder.DropTable(
                name: "EngineeringStatuses");

            migrationBuilder.DropTable(
                name: "FeedbackStatuses");
        }
    }
}
