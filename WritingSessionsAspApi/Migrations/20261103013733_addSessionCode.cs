using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WritingSessionsAspApi.Migrations
{
    /// <inheritdoc />
    public partial class addSessionCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessions_StartTime_StopTime",
                table: "Sessions");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Sessions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Code",
                table: "Sessions",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessions_Code",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Sessions");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_StartTime_StopTime",
                table: "Sessions",
                columns: new[] { "StartTime", "StopTime" },
                unique: true);
        }
    }
}
