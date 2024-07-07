using Chat.Application.Repositories.Interfaces;
using Chat.Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.Hubs
{
    public class ChatRoomHub : Hub
    {
        private readonly IChatRoomRepository _chatRoomRepository;

        public ChatRoomHub(IChatRoomRepository chatRoomRepository)
        {
            _chatRoomRepository = chatRoomRepository;
        }

        public async Task SendMessage(int chatRoomId, string userId, string message)
        {
            var chatRoom = await _chatRoomRepository.GetChatRoomByIdAsync(chatRoomId);
            if (chatRoom == null) throw new HubException("ChatRoom not found");

            var newMessage = new Message
            {
                ChatRoomId = chatRoomId,
                UserId = userId,
                Text = message,
                Timestamp = DateTime.UtcNow
            };

            chatRoom.Messages.Add(newMessage);
            await _chatRoomRepository.CreateChatRoomAsync(chatRoom);
            await Clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", newMessage);
        }

        public async Task JoinConversation(int chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());
        }

        public async Task LeaveConversation(int chatRoomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
        }
    }

}
