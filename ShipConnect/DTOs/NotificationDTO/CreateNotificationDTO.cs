namespace ShipConnect.DTOs.NotificationDTO
{
    public class CreateNotificationDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string? RecipientId { get; set; }
        public IEnumerable<string> RecipientIds { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
