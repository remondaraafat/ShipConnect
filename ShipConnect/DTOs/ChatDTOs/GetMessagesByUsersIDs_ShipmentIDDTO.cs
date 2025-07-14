using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.ChatDTOs
{
    public class GetMessagesByUsersIDs_ShipmentIDDTO
    {
        
        public string Content { get; set; } 
        public DateTime SentAt { get; set; } 
        public bool IsRead { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public int? ShipmentId { get; set; }
    }
}
