using Microsoft.EntityFrameworkCore;
using SIGNALRCHAT.Models;

namespace SIGNALRCHAT.Context
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Models.File> Files { get; set; }
    }
}
