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
            return await _context.ChatRooms
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.ChatRoomId == id);
        }

        public async Task<IEnumerable<ChatRoom>> SearchChatRoomsAsync(string searchTerm)
        {
            return await _context.ChatRooms
                .Where(c => c.Name.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatRoom>> GetChatRoomsAsync()
        {
            return await _context.ChatRooms
                .ToListAsync();
        }

        public async Task<ChatRoom> CreateChatRoomAsync(ChatRoom chatRoom)
        {
            await _context.ChatRooms.AddAsync(chatRoom);
            await _context.SaveChangesAsync();
            return chatRoom;
        }

        public async Task<bool> DeleteChatRoomAsync(int id, string userId)
        {
            var chat = await _context.ChatRooms.FindAsync(id);
            if (chat == null || chat.CreatedBy != userId) return false;

            _context.ChatRooms.Remove(chat);
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
            await _context.SaveChangesAsync();
            await clients.Group(chatRoomId.ToString())
                .SendAsync("ReceiveMessage", newMessage);
        }

        public async Task JoinChatRoomAsync(int chatRoomId, IGroupManager groups, HubCallerContext context, IHubCallerClients clients)
        {
            await groups.AddToGroupAsync(context.ConnectionId, chatRoomId.ToString());
            await clients.Group(chatRoomId.ToString())
                .SendAsync("UserJoined", context.ConnectionId);
        }

        public async Task LeaveChatRoomAsync(int chatRoomId, IGroupManager groups, HubCallerContext context, IHubCallerClients clients)
        {
            await groups.RemoveFromGroupAsync(context.ConnectionId, chatRoomId.ToString());
            await clients.Group(chatRoomId.ToString())
                .SendAsync("UserLeft", context.ConnectionId);
        }
    }
}
