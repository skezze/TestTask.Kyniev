using Chat.Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.Repositories.Interfaces
{
    public interface IChatRoomRepository
    {
        Task<ChatRoom> GetChatRoomByIdAsync(int id);
        Task<IEnumerable<ChatRoom>> GetChatRoomsAsync();
        Task<ChatRoom> CreateChatRoomAsync(ChatRoom chat);
        Task<bool> DeleteChatRoomAsync(int id, string userId);
        Task SendMessageAsync(int chatRoomId, string userId, string message,  IHubCallerClients clients);
        Task JoinChatRoomAsync(int chatRoomId, IGroupManager groups, HubCallerContext context);
        Task LeaveChatRoomAsync(int chatRoomId, IGroupManager groups, HubCallerContext context);
    }
}
