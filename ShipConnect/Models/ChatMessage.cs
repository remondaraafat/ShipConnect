using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Hosting;

namespace ShipConnect.Models
{
    public class ChatMessage:BaseEntity
    {
        [MaxLength(2000)]
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; } = false;
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int? ShipmentId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public ApplicationUser Sender { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public ApplicationUser Receiver { get; set; }

        [ForeignKey(nameof(ShipmentId))]
        public Shipment Shipment { get; set; }
    }
}
