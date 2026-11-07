using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WritingSessionsAspApi.Migrations
{
    /// <inheritdoc />
    public partial class UniqueDateWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessions_StartTime_StopTime",
                table: "Sessions");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Date_Words",
                table: "Sessions",
                columns: new[] { "Date", "Words" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessions_Date_Words",
                table: "Sessions");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_StartTime_StopTime",
                table: "Sessions",
                columns: new[] { "StartTime", "StopTime" },
                unique: true);
        }
    }
}
