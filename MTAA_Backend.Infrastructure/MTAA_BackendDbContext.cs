using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Entities.Chats;
using MTAA_Backend.Domain.Entities.Messages;
using MTAA_Backend.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MTAA_Backend.Infrastructure
{
    public class MTAA_BackendDbContext : IdentityDbContext<User>
    {
        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        public MTAA_BackendDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}