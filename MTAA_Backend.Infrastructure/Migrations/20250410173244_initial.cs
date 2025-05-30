﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: true),
                    EventTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Length = table.Column<long>(type: "bigint", nullable: false),
                    ShortPath = table.Column<string>(type: "text", nullable: false),
                    FullPath = table.Column<string>(type: "text", nullable: false),
                    FileType = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FileMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    VoiceMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    GifMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataCreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                name: "LocationPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSubPoint = table.Column<bool>(type: "boolean", nullable: false),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ZoomLevel = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Coordinates = table.Column<Point>(type: "geography (point)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationPoints_LocationPoints_ParentId",
                        column: x => x.ParentId,
                        principalTable: "LocationPoints",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LocationPoints_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
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
                    Id = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSeen = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false),
                    AvatarId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Visibility = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    IdentificationName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: true),
                    OwnerId = table.Column<string>(type: "text", nullable: true),
                    DataCreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<string>(type: "text", nullable: false),
                    LikesCount = table.Column<int>(type: "integer", nullable: false),
                    CommentsCount = table.Column<int>(type: "integer", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    GlobalScore = table.Column<double>(type: "double precision", nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecommendationFeeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    RecommendationItemsCount = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationFeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendationFeeds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRelationships",
                columns: table => new
                {
                    User1Id = table.Column<string>(type: "text", nullable: false),
                    User2Id = table.Column<string>(type: "text", nullable: false),
                    IsUser1Following = table.Column<bool>(type: "boolean", nullable: false),
                    IsUser2Following = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRelationships", x => new { x.User1Id, x.User2Id });
                    table.ForeignKey(
                        name: "FK_UserRelationships_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRelationships_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VersionItems",
                columns: table => new
                {
                    Type = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionItems", x => new { x.Type, x.UserId });
                    table.ForeignKey(
                        name: "FK_VersionItems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaseGroupUser",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantsId = table.Column<string>(type: "text", nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    GifMessage_FileId = table.Column<Guid>(type: "uuid", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    FileId = table.Column<Guid>(type: "uuid", nullable: true),
                    VoiceMessage_FileId = table.Column<Guid>(type: "uuid", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    DataCreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    LikesCount = table.Column<int>(type: "integer", nullable: false),
                    DislikesCount = table.Column<int>(type: "integer", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<string>(type: "text", nullable: false),
                    ParentCommentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChildCommentsCount = table.Column<int>(type: "integer", nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    DataCreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLikes", x => x.DataCreationTime);
                    table.ForeignKey(
                        name: "FK_PostLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostLikes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostUser",
                columns: table => new
                {
                    WatchedPostsId = table.Column<Guid>(type: "uuid", nullable: false),
                    WatchedUsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUser", x => new { x.WatchedPostsId, x.WatchedUsersId });
                    table.ForeignKey(
                        name: "FK_PostUser_AspNetUsers_WatchedUsersId",
                        column: x => x.WatchedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUser_Posts_WatchedPostsId",
                        column: x => x.WatchedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecommendationItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocalScore = table.Column<double>(type: "double precision", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    FeedId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendationItems_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecommendationItems_RecommendationFeeds_FeedId",
                        column: x => x.FeedId,
                        principalTable: "RecommendationFeeds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SharedRecommendationFeedUser",
                columns: table => new
                {
                    SharedRecommendationFeedsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedRecommendationFeedUser", x => new { x.SharedRecommendationFeedsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SharedRecommendationFeedUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedRecommendationFeedUser_RecommendationFeeds_SharedReco~",
                        column: x => x.SharedRecommendationFeedsId,
                        principalTable: "RecommendationFeeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UserAvatarId = table.Column<string>(type: "text", nullable: true),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: true),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    PostId = table.Column<Guid>(type: "uuid", nullable: true),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupMemberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsNotificationEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    LastMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    UnreadMessagesCount = table.Column<int>(type: "integer", nullable: false)
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
                name: "CommentInteractions",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DataCreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataLastDeleteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataLastEditTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentInteractions", x => new { x.UserId, x.CommentId });
                    table.ForeignKey(
                        name: "FK_CommentInteractions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentInteractions_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShortPath = table.Column<string>(type: "text", nullable: false),
                    FullPath = table.Column<string>(type: "text", nullable: false),
                    FileType = table.Column<string>(type: "text", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    AspectRatio = table.Column<double>(type: "double precision", nullable: false),
                    ImageGroupId = table.Column<Guid>(type: "uuid", nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomAvatarId = table.Column<Guid>(type: "uuid", nullable: true),
                    PresetAvatarId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
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
                columns: new[] { "Id", "ChannelId", "DataCreationTime", "DataLastDeleteTime", "DataLastEditTime", "Discriminator", "IsDeleted", "IsEdited", "MessageId", "Position", "PostId", "Title", "UserAvatarId" },
                values: new object[,]
                {
                    { new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, 0, null, "Preset Avatar", null },
                    { new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, 0, null, "Preset Avatar", null },
                    { new Guid("416c7d33-0a25-4176-b783-64b25919ac12"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, 0, null, "Preset Avatar", null },
                    { new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, 0, null, "Preset Avatar", null },
                    { new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, 0, null, "Preset Avatar", null },
                    { new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "UserPresetAvatarImage", false, false, null, 0, null, "Preset Avatar", null }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "AspectRatio", "FileType", "FullPath", "Height", "ImageGroupId", "ShortPath", "Type", "Width" },
                values: new object[,]
                {
                    { new Guid("2ea5ce43-807b-4419-a506-d68c7cba07a4"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_5_300.jpg", 300, new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"), "userAvatar_5_300", 0, 300 },
                    { new Guid("331f73e4-2035-45fe-9e0d-33a8a930b922"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_3_300.jpg", 300, new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"), "userAvatar_3_300", 0, 300 },
                    { new Guid("3bead263-b9fd-4a6f-a649-c699969863b2"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_5_100.jpg", 100, new Guid("9ad61bee-053b-4042-8b4a-860fe80dd05a"), "userAvatar_5_100", 0, 100 },
                    { new Guid("3efe30aa-4cfa-4003-878d-054de78ea07b"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_4_300.jpg", 300, new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"), "userAvatar_4_300", 0, 300 },
                    { new Guid("4c64dbaf-3ecf-468e-8265-bc9233fc2c7e"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_4_100.jpg", 100, new Guid("79fe4a86-1ca3-4dd0-ad8b-c896bef376ed"), "userAvatar_4_100", 0, 100 },
                    { new Guid("580c86e5-f708-44d2-aba1-d00b6d311ce1"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_3_100.jpg", 100, new Guid("3e4f4c14-f4ae-4238-95b1-075d1e8a9981"), "userAvatar_3_100", 0, 100 },
                    { new Guid("5f3d5283-4fd2-4194-a4d6-345f83c967b3"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_2_100.jpg", 100, new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"), "userAvatar_2_100", 0, 100 },
                    { new Guid("77f98ba4-1961-435c-93c6-c351572e5837"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_1_300.jpg", 300, new Guid("416c7d33-0a25-4176-b783-64b25919ac12"), "userAvatar_1_300", 0, 300 },
                    { new Guid("bf309314-7efa-4b60-a021-0b17a7a5da6f"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_1_100.jpg", 100, new Guid("416c7d33-0a25-4176-b783-64b25919ac12"), "userAvatar_1_100", 0, 100 },
                    { new Guid("d0c6bab5-1b86-4e29-8bcd-34e5ca1d9f2b"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_6_100.jpg", 100, new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"), "userAvatar_6_100", 0, 100 },
                    { new Guid("ed659976-0f06-4d6a-ad9e-9456e4a82d3c"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_2_300.jpg", 300, new Guid("161750a4-9b50-4a1c-a5f1-3221640533c6"), "userAvatar_2_300", 0, 300 },
                    { new Guid("f9ab80f0-cc7f-4f3a-83fe-e25c9e81253b"), 1.0, "jpg", "https://mtaafiles.blob.core.windows.net/images/userAvatar_6_300.jpg", 300, new Guid("d1a56d08-a7de-4855-8a13-5fbda2ca4843"), "userAvatar_6_300", 0, 300 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DataCreationTime_IsDeleted_DisplayName_UserName",
                table: "AspNetUsers",
                columns: new[] { "DataCreationTime", "IsDeleted", "DisplayName", "UserName" });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseMessages_GifMessage_FileId",
                table: "BaseMessages",
                column: "GifMessage_FileId",
                unique: true);

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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentInteractions_CommentId",
                table: "CommentInteractions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentInteractions_Type",
                table: "CommentInteractions",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DataCreationTime",
                table: "Comments",
                column: "DataCreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_LikesCount_DislikesCount_DataCreationTime_IsDeleted",
                table: "Comments",
                columns: new[] { "LikesCount", "DislikesCount", "DataCreationTime", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OwnerId",
                table: "Comments",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageGroups_ChannelId",
                table: "ImageGroups",
                column: "ChannelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageGroups_MessageId",
                table: "ImageGroups",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageGroups_Position",
                table: "ImageGroups",
                column: "Position");

            migrationBuilder.CreateIndex(
                name: "IX_ImageGroups_PostId",
                table: "ImageGroups",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ImageGroupId",
                table: "Images",
                column: "ImageGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Type",
                table: "Images",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_LocationPoints_LocationId",
                table: "LocationPoints",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationPoints_ParentId",
                table: "LocationPoints",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationPoints_Type_ZoomLevel_IsSubPoint_IsVisible",
                table: "LocationPoints",
                columns: new[] { "Type", "ZoomLevel", "IsSubPoint", "IsVisible" });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_EventTime",
                table: "Locations",
                column: "EventTime");

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_PostId",
                table: "PostLikes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_UserId",
                table: "PostLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GlobalScore_CommentsCount_LikesCount_DataCreationTime~",
                table: "Posts",
                columns: new[] { "GlobalScore", "CommentsCount", "LikesCount", "DataCreationTime", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LocationId",
                table: "Posts",
                column: "LocationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_OwnerId",
                table: "Posts",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUser_WatchedUsersId",
                table: "PostUser",
                column: "WatchedUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationFeeds_IsActive",
                table: "RecommendationFeeds",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationFeeds_Type_RecommendationItemsCount",
                table: "RecommendationFeeds",
                columns: new[] { "Type", "RecommendationItemsCount" });

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationFeeds_UserId",
                table: "RecommendationFeeds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationItems_FeedId",
                table: "RecommendationItems",
                column: "FeedId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationItems_LocalScore",
                table: "RecommendationItems",
                column: "LocalScore");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationItems_PostId",
                table: "RecommendationItems",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedRecommendationFeedUser_UsersId",
                table: "SharedRecommendationFeedUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_CustomAvatarId",
                table: "UserAvatars",
                column: "CustomAvatarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_PresetAvatarId",
                table: "UserAvatars",
                column: "PresetAvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupMemberships_GroupId",
                table: "UserGroupMemberships",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupMemberships_IsNotificationEnabled_IsArchived_Unrea~",
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

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationships_IsUser1Following_IsUser2Following",
                table: "UserRelationships",
                columns: new[] { "IsUser1Following", "IsUser2Following" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationships_User2Id",
                table: "UserRelationships",
                column: "User2Id");

            migrationBuilder.CreateIndex(
                name: "IX_VersionItems_UserId",
                table: "VersionItems",
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
                name: "CommentInteractions");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "LocationPoints");

            migrationBuilder.DropTable(
                name: "PostLikes");

            migrationBuilder.DropTable(
                name: "PostUser");

            migrationBuilder.DropTable(
                name: "RecommendationItems");

            migrationBuilder.DropTable(
                name: "SharedRecommendationFeedUser");

            migrationBuilder.DropTable(
                name: "UserGroupMemberships");

            migrationBuilder.DropTable(
                name: "UserRelationships");

            migrationBuilder.DropTable(
                name: "VersionItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "RecommendationFeeds");

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
