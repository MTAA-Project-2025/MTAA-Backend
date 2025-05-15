using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addfirebaseitems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FirebaseItem_AspNetUsers_UserId",
                table: "FirebaseItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FirebaseItem",
                table: "FirebaseItem");

            migrationBuilder.RenameTable(
                name: "FirebaseItem",
                newName: "FirebaseItems");

            migrationBuilder.RenameIndex(
                name: "IX_FirebaseItem_UserId",
                table: "FirebaseItems",
                newName: "IX_FirebaseItems_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FirebaseItems",
                table: "FirebaseItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FirebaseItems_AspNetUsers_UserId",
                table: "FirebaseItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FirebaseItems_AspNetUsers_UserId",
                table: "FirebaseItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FirebaseItems",
                table: "FirebaseItems");

            migrationBuilder.RenameTable(
                name: "FirebaseItems",
                newName: "FirebaseItem");

            migrationBuilder.RenameIndex(
                name: "IX_FirebaseItems_UserId",
                table: "FirebaseItem",
                newName: "IX_FirebaseItem_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FirebaseItem",
                table: "FirebaseItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FirebaseItem_AspNetUsers_UserId",
                table: "FirebaseItem",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
