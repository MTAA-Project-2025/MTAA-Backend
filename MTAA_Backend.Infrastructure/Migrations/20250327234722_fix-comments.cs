using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixcomments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentInteractions_Comments_CommentId",
                table: "CommentInteractions");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentInteractions_Comments_CommentId",
                table: "CommentInteractions",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentInteractions_Comments_CommentId",
                table: "CommentInteractions");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentInteractions_Comments_CommentId",
                table: "CommentInteractions",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
