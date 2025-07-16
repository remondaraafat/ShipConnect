using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.ChatDTOs
{
    public class SendChatMessageRequestDTO
    {
        [Required]

        public string SenderId { get; set; } 
        [Required]

        public string ReceiverId { get; set; } 
        [Required]

        public string Content { get; set; }
        public int? ShipmentId { get; set; }
    }
}
