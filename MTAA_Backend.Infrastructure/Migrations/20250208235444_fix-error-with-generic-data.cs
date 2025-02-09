using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixerrorwithgenericdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"),
                column: "DataCreationTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"),
                column: "DataCreationTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("416c7d33-0a25-4176-b783-64b25919ac12"),
                column: "DataCreationTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"),
                column: "DataCreationTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"),
                column: "DataCreationTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"),
                column: "DataCreationTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"),
                column: "DataCreationTime",
                value: new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(969));

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"),
                column: "DataCreationTime",
                value: new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(970));

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("416c7d33-0a25-4176-b783-64b25919ac12"),
                column: "DataCreationTime",
                value: new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(701));

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"),
                column: "DataCreationTime",
                value: new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(972));

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"),
                column: "DataCreationTime",
                value: new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(973));

            migrationBuilder.UpdateData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"),
                column: "DataCreationTime",
                value: new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(975));
        }
    }
}
