using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WritingSessionsAspApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scenes_Status_StatusId",
                table: "Scenes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Status",
                table: "Status");

            migrationBuilder.RenameTable(
                name: "Status",
                newName: "Statuses");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Sessions",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_AuthorId",
                table: "Sessions",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SceneId",
                table: "Sessions",
                column: "SceneId");

            migrationBuilder.CreateIndex(
                name: "IX_Scenes_ProjectId",
                table: "Scenes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scenes_Projects_ProjectId",
                table: "Scenes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scenes_Statuses_StatusId",
                table: "Scenes",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_AspNetUsers_AuthorId",
                table: "Sessions",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Scenes_SceneId",
                table: "Sessions",
                column: "SceneId",
                principalTable: "Scenes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scenes_Projects_ProjectId",
                table: "Scenes");

            migrationBuilder.DropForeignKey(
                name: "FK_Scenes_Statuses_StatusId",
                table: "Scenes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_AspNetUsers_AuthorId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Scenes_SceneId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_AuthorId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_SceneId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Scenes_ProjectId",
                table: "Scenes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Statuses",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Sessions");

            migrationBuilder.RenameTable(
                name: "Statuses",
                newName: "Status");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Status",
                table: "Status",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Scenes_Status_StatusId",
                table: "Scenes",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
