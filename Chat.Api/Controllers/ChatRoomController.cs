using Chat.Application.Repositories.Interfaces;
using Chat.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChatRoomsController : ControllerBase
    {
        private readonly IChatRoomRepository _chatRoomRepository;

        public ChatRoomsController(IChatRoomRepository chatRoomRepository)
        {
            _chatRoomRepository = chatRoomRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetChatRooms()
        {
            var chatRooms = await _chatRoomRepository.GetChatRoomsAsync();
            return Ok(chatRooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatRoom(int id)
        {
            var chatRoom = await _chatRoomRepository.GetChatRoomByIdAsync(id);
            if (chatRoom == null) return NotFound();
            return Ok(chatRoom);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChatRoom([FromBody] ChatRoom chatRoom)
        {
            var createdChatRoom = await _chatRoomRepository.CreateChatRoomAsync(chatRoom);
            return CreatedAtAction(nameof(GetChatRoom), new { id = createdChatRoom.ChatRoomId }, createdChatRoom);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatRoom(int id, [FromBody] string userId)
        {
            var result = await _chatRoomRepository.DeleteChatRoomAsync(id, userId);
            if (!result) return Forbid("You do not have permission to delete this chat room");
            return NoContent();
        }
    }

}
