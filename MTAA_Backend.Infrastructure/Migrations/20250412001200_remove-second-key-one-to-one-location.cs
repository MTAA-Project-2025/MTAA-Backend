using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removesecondkeyonetoonelocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Locations_LocationId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_LocationId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Posts");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_PostId",
                table: "Locations",
                column: "PostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Posts_PostId",
                table: "Locations",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Posts_PostId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_PostId",
                table: "Locations");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Posts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LocationId",
                table: "Posts",
                column: "LocationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Locations_LocationId",
                table: "Posts",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
