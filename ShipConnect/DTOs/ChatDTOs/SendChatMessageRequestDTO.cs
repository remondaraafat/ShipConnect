namespace ShipConnect.DTOs.ChatDTOs
{
    public class SendChatMessageRequestDTO
    {
        public string SenderId { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
