using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class improveperformence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecommendationFeeds_Type_Weight",
                table: "RecommendationFeeds");

            migrationBuilder.DropIndex(
                name: "IX_Posts_GlobalScore",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "RecommendationItemsCount",
                table: "RecommendationFeeds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CommentsCount",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "ImageGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"),
                column: "Position",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"),
                column: "Position",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("416c7d33-0a25-4176-b783-64b25919ac12"),
                column: "Position",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"),
                column: "Position",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"),
                column: "Position",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"),
                column: "Position",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationFeeds_Type_Weight_RecommendationItemsCount",
                table: "RecommendationFeeds",
                columns: new[] { "Type", "Weight", "RecommendationItemsCount" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GlobalScore_CommentsCount_LikesCount_DataCreationTime_IsDeleted_Description",
                table: "Posts",
                columns: new[] { "GlobalScore", "CommentsCount", "LikesCount", "DataCreationTime", "IsDeleted", "Description" });

            migrationBuilder.CreateIndex(
                name: "IX_Images_Type",
                table: "Images",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ImageGroups_Position",
                table: "ImageGroups",
                column: "Position");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecommendationFeeds_Type_Weight_RecommendationItemsCount",
                table: "RecommendationFeeds");

            migrationBuilder.DropIndex(
                name: "IX_Posts_GlobalScore_CommentsCount_LikesCount_DataCreationTime_IsDeleted_Description",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Images_Type",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_ImageGroups_Position",
                table: "ImageGroups");

            migrationBuilder.DropColumn(
                name: "RecommendationItemsCount",
                table: "RecommendationFeeds");

            migrationBuilder.DropColumn(
                name: "CommentsCount",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "ImageGroups");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationFeeds_Type_Weight",
                table: "RecommendationFeeds",
                columns: new[] { "Type", "Weight" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GlobalScore",
                table: "Posts",
                column: "GlobalScore");
        }
    }
}
