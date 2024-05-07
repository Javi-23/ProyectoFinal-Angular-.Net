using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTFG.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId1",
                table: "Posts",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AppUserId1",
                table: "Posts",
                column: "AppUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AppUserId1",
                table: "Posts",
                column: "AppUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AppUserId1",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AppUserId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AppUserId1",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "UserName");
        }
    }
}
