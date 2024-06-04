using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTFG.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedNamesAtributtes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_postsId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_AspNetUsers_UserId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToFollows_AspNetUsers_FollowedId",
                table: "UserToFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToFollows_AspNetUsers_FollowerId",
                table: "UserToFollows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserToFollows",
                table: "UserToFollows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "UserToFollows",
                newName: "T_UserToFollows");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "T_Posts");

            migrationBuilder.RenameTable(
                name: "Likes",
                newName: "T_Likes");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "T_Comments");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "AspNetUsers",
                newName: "C_Image");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AspNetUsers",
                newName: "C_Description");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "T_UserToFollows",
                newName: "C_FollowerId");

            migrationBuilder.RenameColumn(
                name: "FollowedId",
                table: "T_UserToFollows",
                newName: "C_FollowedId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "T_UserToFollows",
                newName: "C_pk_UserToFollows");

            migrationBuilder.RenameIndex(
                name: "IX_UserToFollows_FollowerId",
                table: "T_UserToFollows",
                newName: "IX_T_UserToFollows_C_FollowerId");

            migrationBuilder.RenameIndex(
                name: "IX_UserToFollows_FollowedId",
                table: "T_UserToFollows",
                newName: "IX_T_UserToFollows_C_FollowedId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "T_Posts",
                newName: "C_UserId");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "T_Posts",
                newName: "C_Text");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "T_Posts",
                newName: "C_Image");

            migrationBuilder.RenameColumn(
                name: "EditDate",
                table: "T_Posts",
                newName: "C_EditDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "T_Posts",
                newName: "C_CreationDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "T_Posts",
                newName: "C_pk_Posts");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_UserId",
                table: "T_Posts",
                newName: "IX_T_Posts_C_UserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "T_Likes",
                newName: "C_UserId");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "T_Likes",
                newName: "C_PostId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "T_Likes",
                newName: "C_pk_Likes");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_UserId",
                table: "T_Likes",
                newName: "IX_T_Likes_C_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_PostId",
                table: "T_Likes",
                newName: "IX_T_Likes_C_PostId");

            migrationBuilder.RenameColumn(
                name: "postsId",
                table: "T_Comments",
                newName: "C_postsId");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "T_Comments",
                newName: "C_UserName");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "T_Comments",
                newName: "C_UserId");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "T_Comments",
                newName: "C_Text");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "T_Comments",
                newName: "C_CreationDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "T_Comments",
                newName: "C_pk_Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "T_Comments",
                newName: "IX_T_Comments_C_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_postsId",
                table: "T_Comments",
                newName: "IX_T_Comments_C_postsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_UserToFollows",
                table: "T_UserToFollows",
                column: "C_pk_UserToFollows");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_Posts",
                table: "T_Posts",
                column: "C_pk_Posts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_Likes",
                table: "T_Likes",
                column: "C_pk_Likes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_Comments",
                table: "T_Comments",
                column: "C_pk_Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Comments_AspNetUsers_C_UserId",
                table: "T_Comments",
                column: "C_UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Comments_T_Posts_C_postsId",
                table: "T_Comments",
                column: "C_postsId",
                principalTable: "T_Posts",
                principalColumn: "C_pk_Posts",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Likes_AspNetUsers_C_UserId",
                table: "T_Likes",
                column: "C_UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Likes_T_Posts_C_PostId",
                table: "T_Likes",
                column: "C_PostId",
                principalTable: "T_Posts",
                principalColumn: "C_pk_Posts",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Posts_AspNetUsers_C_UserId",
                table: "T_Posts",
                column: "C_UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_UserToFollows_AspNetUsers_C_FollowedId",
                table: "T_UserToFollows",
                column: "C_FollowedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_T_UserToFollows_AspNetUsers_C_FollowerId",
                table: "T_UserToFollows",
                column: "C_FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Comments_AspNetUsers_C_UserId",
                table: "T_Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Comments_T_Posts_C_postsId",
                table: "T_Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Likes_AspNetUsers_C_UserId",
                table: "T_Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Likes_T_Posts_C_PostId",
                table: "T_Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Posts_AspNetUsers_C_UserId",
                table: "T_Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_T_UserToFollows_AspNetUsers_C_FollowedId",
                table: "T_UserToFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_T_UserToFollows_AspNetUsers_C_FollowerId",
                table: "T_UserToFollows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_UserToFollows",
                table: "T_UserToFollows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_Posts",
                table: "T_Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_Likes",
                table: "T_Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_Comments",
                table: "T_Comments");

            migrationBuilder.RenameTable(
                name: "T_UserToFollows",
                newName: "UserToFollows");

            migrationBuilder.RenameTable(
                name: "T_Posts",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "T_Likes",
                newName: "Likes");

            migrationBuilder.RenameTable(
                name: "T_Comments",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "C_Image",
                table: "AspNetUsers",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "C_Description",
                table: "AspNetUsers",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "C_FollowerId",
                table: "UserToFollows",
                newName: "FollowerId");

            migrationBuilder.RenameColumn(
                name: "C_FollowedId",
                table: "UserToFollows",
                newName: "FollowedId");

            migrationBuilder.RenameColumn(
                name: "C_pk_UserToFollows",
                table: "UserToFollows",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_T_UserToFollows_C_FollowerId",
                table: "UserToFollows",
                newName: "IX_UserToFollows_FollowerId");

            migrationBuilder.RenameIndex(
                name: "IX_T_UserToFollows_C_FollowedId",
                table: "UserToFollows",
                newName: "IX_UserToFollows_FollowedId");

            migrationBuilder.RenameColumn(
                name: "C_UserId",
                table: "Posts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "C_Text",
                table: "Posts",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "C_Image",
                table: "Posts",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "C_EditDate",
                table: "Posts",
                newName: "EditDate");

            migrationBuilder.RenameColumn(
                name: "C_CreationDate",
                table: "Posts",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "C_pk_Posts",
                table: "Posts",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_T_Posts_C_UserId",
                table: "Posts",
                newName: "IX_Posts_UserId");

            migrationBuilder.RenameColumn(
                name: "C_UserId",
                table: "Likes",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "C_PostId",
                table: "Likes",
                newName: "PostId");

            migrationBuilder.RenameColumn(
                name: "C_pk_Likes",
                table: "Likes",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_T_Likes_C_UserId",
                table: "Likes",
                newName: "IX_Likes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Likes_C_PostId",
                table: "Likes",
                newName: "IX_Likes_PostId");

            migrationBuilder.RenameColumn(
                name: "C_postsId",
                table: "Comments",
                newName: "postsId");

            migrationBuilder.RenameColumn(
                name: "C_UserName",
                table: "Comments",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "C_UserId",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "C_Text",
                table: "Comments",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "C_CreationDate",
                table: "Comments",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "C_pk_Comments",
                table: "Comments",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_T_Comments_C_UserId",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Comments_C_postsId",
                table: "Comments",
                newName: "IX_Comments_postsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserToFollows",
                table: "UserToFollows",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_postsId",
                table: "Comments",
                column: "postsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_AspNetUsers_UserId",
                table: "Likes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToFollows_AspNetUsers_FollowedId",
                table: "UserToFollows",
                column: "FollowedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToFollows_AspNetUsers_FollowerId",
                table: "UserToFollows",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
