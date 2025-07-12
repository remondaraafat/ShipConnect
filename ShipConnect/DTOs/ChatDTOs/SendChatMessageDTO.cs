using ShipConnect.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.ChatDTOs
{
    public class SendChatMessageDTO
    {
        [MaxLength(2000)]
        public string Content { get; set; } = string.Empty;
       
        public bool IsRead { get; set; } = false;
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public int? ShipmentId { get; set; }

    }
}
