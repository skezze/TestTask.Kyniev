using Chat.Api;
using Chat.Domain.DTOs.CRUDDTO;
using Chat.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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
        public async Task DeleteChatRoom_ReturnsNoContent()
        {
            // Arrange
            var chatRoom = new { Name = "Test Chat", CreatedBy = "User1" };
            var postResponse = await _client.PostAsync("/api/ChatRooms/CreateChatRoom",
                new StringContent(JsonSerializer.Serialize(chatRoom), Encoding.UTF8, "application/json"));
            postResponse.EnsureSuccessStatusCode();

            var createdChatRoom = await postResponse.Content.ReadFromJsonAsync<ChatRoom>();

            // Act
            var deleteResponse = await _client.DeleteAsync($"/api/ChatRooms/DeleteChatRoom/{createdChatRoom.ChatRoomId}/{createdChatRoom.CreatedBy}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }

        [TestMethod]
        public async Task DeleteChatRoom_ReturnsUnauthorized_WhenUserHasNoPermission()
        {
            // Arrange
            var chatRoom = new { Name = "Test Chat", CreatedBy = "User1" };
            var postResponse = await _client.PostAsync("/api/ChatRooms/CreateChatRoom",
                new StringContent(JsonSerializer.Serialize(chatRoom), Encoding.UTF8, "application/json"));
            postResponse.EnsureSuccessStatusCode();

            var createdChatRoom = await postResponse.Content.ReadFromJsonAsync<ChatRoom>();

            // Act
            var deleteResponse = await _client.DeleteAsync($"/api/ChatRooms/DeleteChatRoom/{createdChatRoom.ChatRoomId}/AnotherUser");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, deleteResponse.StatusCode);
        }
    }
}


