using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.DTOs
{
    public class MessageDto
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
    }
}
