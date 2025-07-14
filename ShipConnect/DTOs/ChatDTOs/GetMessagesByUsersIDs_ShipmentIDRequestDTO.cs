namespace ShipConnect.DTOs.ChatDTOs
{
    public class GetMessagesByUsersIDs_ShipmentIDRequestDTO
    {
        public string SenderId { get; set; } 
        public string ReceiverId { get; set; } 
        public int? ShipmentId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
