using Chat.Application.Repositories.Interfaces;
using Chat.Data;
using Chat.Domain.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.Repositories
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

        public async Task SendMessageAsync(int chatRoomId, string userId, string message, IHubCallerClients clients )
        {
            var chatRoom = await GetChatRoomByIdAsync(chatRoomId);
            if (chatRoom == null) throw new HubException("ChatRoom not found");

            var newMessage = new Message
            {
                ChatRoomId = chatRoomId,
                UserId = userId,
                Text = message,
                Timestamp = DateTime.UtcNow
            };

            chatRoom.Messages.Add(newMessage);
            await CreateChatRoomAsync(chatRoom);
            await clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", newMessage);
        }

        public async Task JoinChatRoomAsync(int chatRoomId, IGroupManager groups, HubCallerContext context)
        {
            await groups.AddToGroupAsync(context.ConnectionId, chatRoomId.ToString());
        }

        public async Task LeaveChatRoomAsync(int chatRoomId, IGroupManager groups, HubCallerContext context)
        {
            await groups.RemoveFromGroupAsync(context.ConnectionId, chatRoomId.ToString());
        }
    }
}
