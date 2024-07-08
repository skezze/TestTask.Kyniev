using Chat.Application.Hubs.Interfaces;
using Chat.Application.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.Hubs
{
    public class ChatRoomHub : Hub,IChatRoomHub
    {
        private readonly IChatRoomRepository _chatRoomRepository;

        public ChatRoomHub(IChatRoomRepository chatRoomRepository)
        {
            _chatRoomRepository = chatRoomRepository;
        }

        public async Task SendMessage(int chatRoomId, string userId, string message)
        {
            await _chatRoomRepository.SendMessageAsync(chatRoomId, userId, message, this.Clients);
        }

        public async Task JoinChatRoom(int chatRoomId)
        {
            await _chatRoomRepository.JoinChatRoomAsync(chatRoomId, this.Groups, this.Context, this.Clients);
        }

        public async Task LeaveChatRoom(int chatRoomId)
        {
            await _chatRoomRepository.LeaveChatRoomAsync(chatRoomId, this.Groups, this.Context, this.Clients);
        }
    }

}
