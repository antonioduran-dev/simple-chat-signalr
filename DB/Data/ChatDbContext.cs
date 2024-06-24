using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }

        // defines the entities
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // creates the relationships
            modelBuilder.Entity<User>()
            .HasMany(u => u.Messages)
            .WithOne(m => m.User)
            .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<User>(tb =>
            {
                tb.Property(col => col.Username).HasMaxLength(20);
            });

            modelBuilder.Entity<Message>(tb =>
            {
                tb.Property(col => col.Content).HasMaxLength(200);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
