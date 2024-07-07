namespace Chat.Domain.Models
{
    public class ChatRoom
    {
        public int ChatRoomId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
