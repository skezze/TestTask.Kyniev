namespace Chat.Domain.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int ChatRoomId { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}
