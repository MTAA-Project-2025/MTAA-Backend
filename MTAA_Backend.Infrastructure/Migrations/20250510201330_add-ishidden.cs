using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addishidden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_GlobalScore_CommentsCount_LikesCount_DataCreationTime~",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "HiddenReason",
                table: "Posts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Posts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ScheduleJobId",
                table: "Posts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SchedulePublishDate",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GlobalScore_CommentsCount_LikesCount_DataCreationTime~",
                table: "Posts",
                columns: new[] { "GlobalScore", "CommentsCount", "LikesCount", "DataCreationTime", "IsDeleted", "IsHidden", "SchedulePublishDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_GlobalScore_CommentsCount_LikesCount_DataCreationTime~",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "HiddenReason",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ScheduleJobId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "SchedulePublishDate",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GlobalScore_CommentsCount_LikesCount_DataCreationTime~",
                table: "Posts",
                columns: new[] { "GlobalScore", "CommentsCount", "LikesCount", "DataCreationTime", "IsDeleted" });
        }
    }
}
