using Chat.Domain.Models;

namespace Chat.Application.Repositories.Interfaces
{
    public interface IChatRoomRepository
    {
        Task<ChatRoom> GetChatRoomByIdAsync(int id);
        Task<IEnumerable<ChatRoom>> GetChatRoomsAsync();
        Task<ChatRoom> CreateChatRoomAsync(ChatRoom chat);
        Task<bool> DeleteChatRoomAsync(int id, string userId);
    }
}
