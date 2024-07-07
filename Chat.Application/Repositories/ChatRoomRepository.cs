using Chat.Common.Repositories.Interfaces;
using Chat.Data;
using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Chat.Common.Services
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly ChatDbContext _context;

        public ChatRoomRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<ChatRoom> GetChatRoomByIdAsync(int id)
        {
            return await _context.Chats.Include(c => c.Messages).FirstOrDefaultAsync(c => c.ChatRoomId == id);
        }

        public async Task<IEnumerable<ChatRoom>> GetChatRoomsAsync()
        {
            return await _context.Chats.Include(c => c.Messages).ToListAsync();
        }

        public async Task<ChatRoom> CreateChatRoomAsync(ChatRoom chatRoom)
        {
            await _context.Chats.AddAsync(chatRoom);
            await _context.SaveChangesAsync();
            return chatRoom;
        }

        public async Task<bool> DeleteChatRoomAsync(int id, string userId)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat == null || chat.CreatedBy != userId) return false;

            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
