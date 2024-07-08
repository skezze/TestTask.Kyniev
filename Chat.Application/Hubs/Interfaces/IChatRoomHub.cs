namespace Chat.Application.Hubs.Interfaces;

public interface IChatRoomHub
{
    Task SendMessage(int chatRoomId, string userId, string message);
    Task JoinChatRoom(int chatRoomId);
    Task LeaveChatRoom(int chatRoomId);
    
}