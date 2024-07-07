using System.Net.Http.Json;
using Chat.Api;
using Chat.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Chat.Tests
{
    [TestClass]
    public class ChatRoomsControllerTests
    {
        private readonly HttpClient _client;

        public ChatRoomsControllerTests()
        {
            var factory = new WebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        [TestMethod]
        public async Task GetChatRooms_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/ChatRooms/GetChatRooms");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(content);
        }

        [TestMethod]
        public async Task CreateChatRoom_ReturnsCreated()
        {
            var chatRoom = new ChatRoom { Name = "Test Chat", CreatedBy = "User1" };

            var response = await _client.PostAsJsonAsync("/api/ChatRooms/CreateChatRoom", chatRoom);
            response.EnsureSuccessStatusCode();

            var createdChatRoom = await response.Content.ReadFromJsonAsync<ChatRoom>();
            Assert.IsNotNull(createdChatRoom);
            Assert.AreEqual("Test Chat", createdChatRoom.Name);
        }

        [TestMethod]
        public async Task DeleteConversation_ReturnsNoContent()
        {
            var chatRoom = new ChatRoom { Name = "Test Chat", CreatedBy = "User1" };
            var postResponse = await _client.PostAsJsonAsync("/api/ChatRooms/CreateChatRoom", chatRoom);
            postResponse.EnsureSuccessStatusCode();

            var createdChatRoom = await postResponse.Content.ReadFromJsonAsync<ChatRoom>();
            var deleteResponse = await _client.DeleteAsync($"/api/ChatRooms/DeleteChatRoom{createdChatRoom.ChatRoomId}");

            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}

