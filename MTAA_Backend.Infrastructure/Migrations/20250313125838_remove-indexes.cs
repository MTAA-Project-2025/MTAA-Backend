using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeindexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecommendationFeeds_Type_Weight_RecommendationItemsCount",
                table: "RecommendationFeeds");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationFeeds_Type_RecommendationItemsCount",
                table: "RecommendationFeeds",
                columns: new[] { "Type", "RecommendationItemsCount" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RecommendationFeeds_Type_RecommendationItemsCount",
                table: "RecommendationFeeds");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationFeeds_Type_Weight_RecommendationItemsCount",
                table: "RecommendationFeeds",
                columns: new[] { "Type", "Weight", "RecommendationItemsCount" });
        }
    }
}
