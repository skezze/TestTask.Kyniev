using Chat.Application.Repositories.Interfaces;
using Chat.Domain.DTOs.CRUDDTO;
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
        
        [HttpGet]
        public async Task<IActionResult> SearchChatRooms([FromQuery] string searchTerm)
        {
            var chatRooms = await _chatRoomRepository.SearchChatRoomsAsync(searchTerm);
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
        public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomDto createChatRoomDto)
        {

            var createdChatRoom = await _chatRoomRepository.CreateChatRoomAsync(new ChatRoom
            {
                Name = createChatRoomDto.Name,
                CreatedBy = createChatRoomDto.CreatedBy,
                Messages = new List<Message>()
            });
            return CreatedAtAction(nameof(GetChatRoom), new { id = createdChatRoom.ChatRoomId }, createdChatRoom);
        }

        [HttpDelete("{id}/{userId}")]
        public async Task<IActionResult> DeleteChatRoom(int id, string userId)
        {
            var result = await _chatRoomRepository.DeleteChatRoomAsync(id, userId);
            if (!result) return Unauthorized("You do not have permission to delete this chat room");
            return NoContent();
        }
    }

}
