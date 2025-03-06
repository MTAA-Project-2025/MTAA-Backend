using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MTAA_Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    ShortPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VoiceMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GifMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    AvatarId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Visibility = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdentificationName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseGroups_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GlobalScore = table.Column<double>(type: "float", nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecomendationFeeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecomendationFeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecomendationFeeds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContactId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    ContactType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContacts_AspNetUsers_ContactId",
                        column: x => x.ContactId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserContacts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "BaseMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    GifMessage_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VoiceMessage_FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseMessages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseMessages_BaseGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "BaseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseMessages_MyFiles_FileId",
                        column: x => x.FileId,
                        principalTable: "MyFiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseMessages_MyFiles_GifMessage_FileId",
                        column: x => x.GifMessage_FileId,
                        principalTable: "MyFiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseMessages_MyFiles_VoiceMessage_FileId",
                        column: x => x.VoiceMessage_FileId,
                        principalTable: "MyFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostUser",
                columns: table => new
                {
                    LikedPostsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikedUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUser", x => new { x.LikedPostsId, x.LikedUsersId });
                    table.ForeignKey(
                        name: "FK_PostUser_AspNetUsers_LikedUsersId",
                        column: x => x.LikedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUser_Posts_LikedPostsId",
                        column: x => x.LikedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PostUser1",
                columns: table => new
                {
                    WatchedPostsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WatchedUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUser1", x => new { x.WatchedPostsId, x.WatchedUsersId });
                    table.ForeignKey(
                        name: "FK_PostUser1_AspNetUsers_WatchedUsersId",
                        column: x => x.WatchedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUser1_Posts_WatchedPostsId",
                        column: x => x.WatchedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RecomendationItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocalScore = table.Column<double>(type: "float", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecomendationItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecomendationItems_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecomendationItems_RecomendationFeeds_FeedId",
                        column: x => x.FeedId,
                        principalTable: "RecomendationFeeds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImageGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAvatarId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageGroups_BaseGroups_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "BaseGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImageGroups_BaseMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "BaseMessages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImageGroups_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShortPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    AspectRatio = table.Column<double>(type: "float", nullable: false),
                    ImageGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_ImageGroups_ImageGroupId",
                        column: x => x.ImageGroupId,
                        principalTable: "ImageGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                columns: new[] { "Id", "ChannelId", "DataCreationTime", "DataLastDeleteTime", "DataLastEditTime", "Discriminator", "IsDeleted", "IsEdited", "MessageId", "PostId", "Title", "UserAvatarId" },
                values: new object[,]
                {
                    { new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, null, "Preset Avatar", null },
                    { new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, null, "Preset Avatar", null },
                    { new Guid("416c7d33-0a25-4176-b783-64b25919ac12"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, null, "Preset Avatar", null },
                    { new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, null, "Preset Avatar", null },
                    { new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, null, "Preset Avatar", null },
                    { new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, null, "Preset Avatar", null }
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId",
                unique: true,
                filter: "[AvatarId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseGroups_IdentificationName_DisplayName",
                table: "BaseGroups",
                columns: new[] { "IdentificationName", "DisplayName" });

            migrationBuilder.CreateIndex(
                name: "IX_BaseGroups_OwnerId",
                table: "BaseGroups",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseGroups_Type_Visibility",
                table: "BaseGroups",
                columns: new[] { "Type", "Visibility" });

            migrationBuilder.CreateIndex(
                name: "IX_BaseGroupUser_ParticipantsId",
                table: "BaseGroupUser",
                column: "ParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_FileId",
                table: "BaseMessages",
                column: "FileId",
                unique: true,
                filter: "[FileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_GifMessage_FileId",
                table: "BaseMessages",
                column: "GifMessage_FileId",
                unique: true,
                filter: "[FileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_GroupId",
                table: "BaseMessages",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_SenderId",
                table: "BaseMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_Type_IsRead",
                table: "BaseMessages",
                columns: new[] { "Type", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_VoiceMessage_FileId",
                table: "BaseMessages",
                column: "VoiceMessage_FileId",
                unique: true,
                filter: "[FileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageGroups_ChannelId",
                table: "ImageGroups",
                column: "ChannelId",
                unique: true,
                filter: "[ChannelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ImageGroups_MessageId",
                table: "ImageGroups",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageGroups_PostId",
                table: "ImageGroups",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ImageGroupId",
                table: "Images",
                column: "ImageGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GlobalScore",
                table: "Posts",
                column: "GlobalScore");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LocationId",
                table: "Posts",
                column: "LocationId",
                unique: true,
                filter: "[LocationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_OwnerId",
                table: "Posts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUser_LikedUsersId",
                table: "PostUser",
                column: "LikedUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUser1_WatchedUsersId",
                table: "PostUser1",
                column: "WatchedUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_RecomendationFeeds_Type_Weight",
                table: "RecomendationFeeds",
                columns: new[] { "Type", "Weight" });

            migrationBuilder.CreateIndex(
                name: "IX_RecomendationFeeds_UserId",
                table: "RecomendationFeeds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecomendationItems_FeedId",
                table: "RecomendationItems",
                column: "FeedId");

            migrationBuilder.CreateIndex(
                name: "IX_RecomendationItems_LocalScore",
                table: "RecomendationItems",
                column: "LocalScore");

            migrationBuilder.CreateIndex(
                name: "IX_RecomendationItems_PostId",
                table: "RecomendationItems",
                column: "PostId");

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

            migrationBuilder.CreateIndex(
                name: "IX_UserContacts_ContactId",
                table: "UserContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContacts_UserId",
                table: "UserContacts",
                column: "UserId");

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
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_BaseGroups_AspNetUsers_OwnerId",
                table: "BaseGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseMessages_AspNetUsers_SenderId",
                table: "BaseMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_OwnerId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BaseGroupUser");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "PostUser");

            migrationBuilder.DropTable(
                name: "PostUser1");

            migrationBuilder.DropTable(
                name: "RecomendationItems");

            migrationBuilder.DropTable(
                name: "UserContacts");

            migrationBuilder.DropTable(
                name: "UserGroupMemberships");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "RecomendationFeeds");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserAvatars");

            migrationBuilder.DropTable(
                name: "ImageGroups");

            migrationBuilder.DropTable(
                name: "BaseMessages");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "BaseGroups");

            migrationBuilder.DropTable(
                name: "MyFiles");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
