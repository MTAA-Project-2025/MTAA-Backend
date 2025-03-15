using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeuserrelationshipnaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUser2Follow",
                table: "UserRelationships",
                newName: "IsUser2Following");

            migrationBuilder.RenameColumn(
                name: "IsUser1Follow",
                table: "UserRelationships",
                newName: "IsUser1Following");

            migrationBuilder.RenameIndex(
                name: "IX_UserRelationships_IsUser1Follow_IsUser2Follow",
                table: "UserRelationships",
                newName: "IX_UserRelationships_IsUser1Following_IsUser2Following");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUser2Following",
                table: "UserRelationships",
                newName: "IsUser2Follow");

            migrationBuilder.RenameColumn(
                name: "IsUser1Following",
                table: "UserRelationships",
                newName: "IsUser1Follow");

            migrationBuilder.RenameIndex(
                name: "IX_UserRelationships_IsUser1Following_IsUser2Following",
                table: "UserRelationships",
                newName: "IX_UserRelationships_IsUser1Follow_IsUser2Follow");
        }
    }
}
