using Chat.Application.Repositories;
using Chat.Data;
using Chat.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.Tests
{
        [TestClass]
        public class ChatRoomRepositoryTests
        {
            private DbContextOptions<ChatDbContext> _options;
            private ChatDbContext _context;
            private ChatRoomRepository _repository;

            [TestInitialize]
            public void Initialize()
            {
                _options = new DbContextOptionsBuilder<ChatDbContext>()
                    .UseInMemoryDatabase(databaseName: "ChatDatabase")
                    .Options;

                _context = new ChatDbContext(_options);
                _repository = new ChatRoomRepository(_context);
            }

            [TestMethod]
            public async Task CreateChatRoomAsync_ShouldAddChatRoom()
            {
                var chatRoom = new ChatRoom { Name = "Test Chat", CreatedBy = "User1" };
                var createdChatRoom = await _repository.CreateChatRoomAsync(chatRoom);

                Assert.IsNotNull(createdChatRoom);
                Assert.AreEqual("Test Chat", createdChatRoom.Name);
            }

            [TestMethod]
            public async Task GetChatRoomByIdAsync_ShouldReturnChatRoom()
            {
                var chatRoom = new ChatRoom { Name = "Test Chat", CreatedBy = "User1" };
                await _repository.CreateChatRoomAsync(chatRoom);

                var retrievedChatRoom = await _repository.GetChatRoomByIdAsync(chatRoom.ChatRoomId);

                Assert.IsNotNull(retrievedChatRoom);
                Assert.AreEqual("Test Chat", retrievedChatRoom.Name);
            }

            [TestMethod]
            public async Task DeleteChatRoomAsync_ShouldDeleteChatRoom()
            {
                var chatRoom = new ChatRoom { Name = "Test Chat", CreatedBy = "User1" };
                var createdChatRoom = await _repository.CreateChatRoomAsync(chatRoom);

                var result = await _repository.DeleteChatRoomAsync(createdChatRoom.ChatRoomId, "User1");

                Assert.IsTrue(result);
                var deletedChatRoom = await _repository.GetChatRoomByIdAsync(createdChatRoom.ChatRoomId);
                Assert.IsNull(deletedChatRoom);
            }
        }
}

