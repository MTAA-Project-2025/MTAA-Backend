using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addposts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "ImageGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostUser",
                columns: table => new
                {
                    LikedPostsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikedUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUser", x => new { x.LikedPostsId, x.LikedUsersId });
                    table.ForeignKey(
                        name: "FK_PostUser_AspNetUsers_LikedUsersId",
                        column: x => x.LikedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUser_Posts_LikedPostsId",
                        column: x => x.LikedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"),
                column: "PostId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"),
                column: "PostId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("416c7d33-0a25-4176-b783-64b25919ac12"),
                column: "PostId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"),
                column: "PostId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"),
                column: "PostId",
                value: null);

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"),
                column: "PostId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_ImageGroups_PostId",
                table: "ImageGroups",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LocationId",
                table: "Posts",
                column: "LocationId",
                unique: true,
                filter: "[LocationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_OwnerId",
                table: "Posts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUser_LikedUsersId",
                table: "PostUser",
                column: "LikedUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageGroups_Posts_PostId",
                table: "ImageGroups",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageGroups_Posts_PostId",
                table: "ImageGroups");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "PostUser");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_ImageGroups_PostId",
                table: "ImageGroups");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "ImageGroups");
        }
    }
}
