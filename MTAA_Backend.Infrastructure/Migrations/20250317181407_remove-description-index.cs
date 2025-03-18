using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedescriptionindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_GlobalScore_CommentsCount_LikesCount_DataCreationTime_IsDeleted_Description",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GlobalScore_CommentsCount_LikesCount_DataCreationTime_IsDeleted",
                table: "Posts",
                columns: new[] { "GlobalScore", "CommentsCount", "LikesCount", "DataCreationTime", "IsDeleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_GlobalScore_CommentsCount_LikesCount_DataCreationTime_IsDeleted",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GlobalScore_CommentsCount_LikesCount_DataCreationTime_IsDeleted_Description",
                table: "Posts",
                columns: new[] { "GlobalScore", "CommentsCount", "LikesCount", "DataCreationTime", "IsDeleted", "Description" });
        }
    }
}
