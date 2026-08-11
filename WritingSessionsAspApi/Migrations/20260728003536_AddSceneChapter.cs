using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WritingSessionsAspApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSceneChapter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Chapter",
                table: "Scenes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chapter",
                table: "Scenes");
        }
    }
}
