using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.Data
{
    public class ChatDbContext:DbContext
    {
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatRoom>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.ChatRoom)
                .HasForeignKey(m => m.ChatRoomId);
        }
    }
}
