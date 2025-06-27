using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.Models
{
    public class Notification:BaseEntity
    {
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public NotificationType Type { get; set; } = NotificationType.General;
        public string RecipientId { get; set; }

        [ForeignKey(nameof(RecipientId))]
        public ApplicationUser Recipient { get; set; }        // اللي هيوصله الإشعار


    }
}
