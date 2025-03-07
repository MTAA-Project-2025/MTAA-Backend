using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Entities.Messages;
using Microsoft.EntityFrameworkCore;
using MTAA_Backend.Domain.Entities.Images;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MTAA_Backend.Infrastructure.Configuration.Images;
using MTAA_Backend.Domain.Entities.Groups;
using MTAA_Backend.Domain.Entities.Files;

namespace MTAA_Backend.Infrastructure
{
    public class MTAA_BackendDbContext : IdentityDbContext<User>
    {
        #region Groups
        public DbSet<BaseGroup> BaseGroups { get; set; }
        public DbSet<ContactChat> ContactChats { get; set; }
        public DbSet<Channel> Channels { get; set; }

        public DbSet<UserGroupMembership> UserGroupMemberships { get; set; }
        #endregion

        #region Messages
        public DbSet<BaseMessage> BaseMessages { get; set; }
        public DbSet<FileMessage> FileMessages { get; set; }
        public DbSet<GifMessage> GifMessages { get; set; }
        public DbSet<ImagesMessage> ImagesMessages { get; set; }
        public DbSet<TextMessage> TextMessages { get; set; }
        public DbSet<VoiceMessage> VoiceMessages { get; set; }
        #endregion



        public DbSet<MyFile> MyFiles { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<UserAvatar> UserAvatars { get; set; }

        public DbSet<MyImageGroup> ImageGroups { get; set; }
        public DbSet<UserPresetAvatarImage> UserPresetAvatarImages { get; set; }
        public DbSet<MyImage> Images { get; set; }

        public DbSet<UserRelationship> UserRelationships { get; set; }


        public MTAA_BackendDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyImageConfiguration).Assembly);
        }
    }
}