using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WritingSessionsAspApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionAuthorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_AspNetUsers_AuthorId",
                table: "Sessions");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Sessions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_AspNetUsers_AuthorId",
                table: "Sessions",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_AspNetUsers_AuthorId",
                table: "Sessions");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Sessions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_AspNetUsers_AuthorId",
                table: "Sessions",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
