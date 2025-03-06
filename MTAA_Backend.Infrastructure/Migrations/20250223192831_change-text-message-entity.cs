using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changetextmessageentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortText",
                table: "BaseMessages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortText",
                table: "BaseMessages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
