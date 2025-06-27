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
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public int? ShipmentId { get; set; }

        [ForeignKey(nameof(SenderId))]
        [InverseProperty(nameof(ApplicationUser.SentMessages))]
        public ApplicationUser Sender { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        [InverseProperty(nameof(ApplicationUser.ReceivedMessages))]
        public ApplicationUser Receiver { get; set; }

        [ForeignKey(nameof(ShipmentId))]
        public Shipment Shipment { get; set; }
    }
}
