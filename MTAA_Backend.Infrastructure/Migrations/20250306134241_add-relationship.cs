using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRelationship",
                columns: table => new
                {
                    User1Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User2Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsUser1Followig = table.Column<bool>(type: "bit", nullable: false),
                    IsUser2Followig = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRelationship", x => new { x.User1Id, x.User2Id });
                    table.ForeignKey(
                        name: "FK_UserRelationship_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRelationship_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationship_IsUser1Followig_IsUser2Followig",
                table: "UserRelationship",
                columns: new[] { "IsUser1Followig", "IsUser2Followig" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationship_User2Id",
                table: "UserRelationship",
                column: "User2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRelationship");
        }
    }
}
