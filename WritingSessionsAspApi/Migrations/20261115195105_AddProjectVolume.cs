using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WritingSessionsAspApi.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectVolume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Volume",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Projects");
        }
    }
}
