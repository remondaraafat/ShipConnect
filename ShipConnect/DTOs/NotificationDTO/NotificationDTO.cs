namespace ShipConnect.DTOs.NotificationDTO
{
    public class NotificationDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
