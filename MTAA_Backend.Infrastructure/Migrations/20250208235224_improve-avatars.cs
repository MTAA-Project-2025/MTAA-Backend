using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class improveavatars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ImageGroups_AvatarId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ImageGroups",
                newName: "UserAvatarId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ImageGroups",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "UserAvatars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomAvatarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PresetAvatarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvatars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAvatars_ImageGroups_CustomAvatarId",
                        column: x => x.CustomAvatarId,
                        principalTable: "ImageGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAvatars_ImageGroups_PresetAvatarId",
                        column: x => x.PresetAvatarId,
                        principalTable: "ImageGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ImageGroups",
                columns: new[] { "Id", "DataCreationTime", "DataLastDeleteTime", "DataLastEditTime", "Discriminator", "IsDeleted", "IsEdited", "Title", "UserAvatarId" },
                values: new object[,]
                {
                    { new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"), new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(969), null, null, "UserPresetAvatarImage", false, false, "Preset Avatar", null },
                    { new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"), new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(970), null, null, "UserPresetAvatarImage", false, false, "Preset Avatar", null },
                    { new Guid("416c7d33-0a25-4176-b783-64b25919ac12"), new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(701), null, null, "UserPresetAvatarImage", false, false, "Preset Avatar", null },
                    { new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"), new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(972), null, null, "UserPresetAvatarImage", false, false, "Preset Avatar", null },
                    { new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"), new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(973), null, null, "UserPresetAvatarImage", false, false, "Preset Avatar", null },
                    { new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"), new DateTime(2025, 2, 8, 23, 52, 24, 229, DateTimeKind.Utc).AddTicks(975), null, null, "UserPresetAvatarImage", false, false, "Preset Avatar", null }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "AspectRatio", "FileType", "FullPath", "Height", "ImageGroupId", "ShortPath", "Width" },
                values: new object[,]
                {
                    { new Guid("2ea5ce43-807b-4419-a506-d68c7cba07a4"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_5_300.jpg", 300, new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"), "userAvatar_5_300", 300 },
                    { new Guid("331f73e4-2035-45fe-9e0d-33a8a930b922"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_3_300.jpg", 300, new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"), "userAvatar_3_300", 300 },
                    { new Guid("3bead263-b9fd-4a6f-a649-c699969863b2"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_5_100.jpg", 100, new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"), "userAvatar_5_100", 100 },
                    { new Guid("3efe30aa-4cfa-4003-878d-054de78ea07b"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_4_300.jpg", 300, new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"), "userAvatar_4_300", 300 },
                    { new Guid("4c64dbaf-3ecf-468e-8265-bc9233fc2c7e"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_4_100.jpg", 100, new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"), "userAvatar_4_100", 100 },
                    { new Guid("580c86e5-f708-44d2-aba1-d00b6d311ce1"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_3_100.jpg", 100, new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"), "userAvatar_3_100", 100 },
                    { new Guid("5f3d5283-4fd2-4194-a4d6-345f83c967b3"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_2_100.jpg", 100, new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"), "userAvatar_2_100", 100 },
                    { new Guid("77f98ba4-1961-435c-93c6-c351572e5837"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_1_300.jpg", 300, new Guid("416c7d33-0a25-4176-b783-64b25919ac12"), "userAvatar_1_300", 300 },
                    { new Guid("bf309314-7efa-4b60-a021-0b17a7a5da6f"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_1_100.jpg", 100, new Guid("416c7d33-0a25-4176-b783-64b25919ac12"), "userAvatar_1_100", 100 },
                    { new Guid("d0c6bab5-1b86-4e29-8bcd-34e5ca1d9f2b"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_6_100.jpg", 100, new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"), "userAvatar_6_100", 100 },
                    { new Guid("ed659976-0f06-4d6a-ad9e-9456e4a82d3c"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_2_300.jpg", 300, new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"), "userAvatar_2_300", 300 },
                    { new Guid("f9ab80f0-cc7f-4f3a-83fe-e25c9e81253b"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_6_300.jpg", 300, new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"), "userAvatar_6_300", 300 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_CustomAvatarId",
                table: "UserAvatars",
                column: "CustomAvatarId",
                unique: true,
                filter: "[CustomAvatarId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_PresetAvatarId",
                table: "UserAvatars",
                column: "PresetAvatarId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserAvatars_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId",
                principalTable: "UserAvatars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserAvatars_AvatarId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserAvatars");

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("2ea5ce43-807b-4419-a506-d68c7cba07a4"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("331f73e4-2035-45fe-9e0d-33a8a930b922"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("3bead263-b9fd-4a6f-a649-c699969863b2"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("3efe30aa-4cfa-4003-878d-054de78ea07b"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("4c64dbaf-3ecf-468e-8265-bc9233fc2c7e"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("580c86e5-f708-44d2-aba1-d00b6d311ce1"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("5f3d5283-4fd2-4194-a4d6-345f83c967b3"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("77f98ba4-1961-435c-93c6-c351572e5837"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("bf309314-7efa-4b60-a021-0b17a7a5da6f"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("d0c6bab5-1b86-4e29-8bcd-34e5ca1d9f2b"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("ed659976-0f06-4d6a-ad9e-9456e4a82d3c"));

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("f9ab80f0-cc7f-4f3a-83fe-e25c9e81253b"));

            migrationBuilder.DeleteData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"));

            migrationBuilder.DeleteData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"));

            migrationBuilder.DeleteData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("416c7d33-0a25-4176-b783-64b25919ac12"));

            migrationBuilder.DeleteData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"));

            migrationBuilder.DeleteData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"));

            migrationBuilder.DeleteData(
                table: "ImageGroups",
                keyColumn: "Id",
                keyValue: new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"));

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ImageGroups");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserAvatarId",
                table: "ImageGroups",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ImageGroups_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId",
                principalTable: "ImageGroups",
                principalColumn: "Id");
        }
    }
}
