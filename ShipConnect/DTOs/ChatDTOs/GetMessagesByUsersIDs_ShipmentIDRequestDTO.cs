using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.ChatDTOs
{
    public class GetMessagesByUsersIDs_ShipmentIDRequestDTO
    {
        [Required]
        public string SenderId { get; set; }
        [Required]

        public string ReceiverId { get; set; }
        [Required]

        public int? ShipmentId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
