using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addgroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseMessages_Chats_ChatId",
                table: "BaseMessages");

            migrationBuilder.DropTable(
                name: "ChatUser");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "BaseMessages",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_BaseMessages_ChatId",
                table: "BaseMessages",
                newName: "IX_BaseMessages_GroupId");

            migrationBuilder.CreateTable(
                name: "BaseGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Visibility = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseGroupUser",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseGroupUser", x => new { x.GroupsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_BaseGroupUser_AspNetUsers_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseGroupUser_BaseGroups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "BaseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupMemberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsNotificationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    LastMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnreadMessagesCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupMemberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroupMemberships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupMemberships_BaseGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "BaseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupMemberships_BaseMessages_LastMessageId",
                        column: x => x.LastMessageId,
                        principalTable: "BaseMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseGroups_Type",
                table: "BaseGroups",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_BaseGroupUser_ParticipantsId",
                table: "BaseGroupUser",
                column: "ParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupMemberships_GroupId",
                table: "UserGroupMemberships",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupMemberships_IsNotificationEnabled_IsArchived_UnreadMessagesCount",
                table: "UserGroupMemberships",
                columns: new[] { "IsNotificationEnabled", "IsArchived", "UnreadMessagesCount" });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupMemberships_LastMessageId",
                table: "UserGroupMemberships",
                column: "LastMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupMemberships_UserId",
                table: "UserGroupMemberships",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseMessages_BaseGroups_GroupId",
                table: "BaseMessages",
                column: "GroupId",
                principalTable: "BaseGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseMessages_BaseGroups_GroupId",
                table: "BaseMessages");

            migrationBuilder.DropTable(
                name: "BaseGroupUser");

            migrationBuilder.DropTable(
                name: "UserGroupMemberships");

            migrationBuilder.DropTable(
                name: "BaseGroups");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "BaseMessages",
                newName: "ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_BaseMessages_GroupId",
                table: "BaseMessages",
                newName: "IX_BaseMessages_ChatId");

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatUser",
                columns: table => new
                {
                    ChatsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUser", x => new { x.ChatsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_ChatUser_AspNetUsers_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUser_Chats_ChatsId",
                        column: x => x.ChatsId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_ParticipantsId",
                table: "ChatUser",
                column: "ParticipantsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseMessages_Chats_ChatId",
                table: "BaseMessages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
