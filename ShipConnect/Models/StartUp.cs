using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipConnect.Models
{
    public class StartUp:BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        [Phone]
        public string? Phone { get; set; }
        [StringLength(100)]
        public string? Website { get; set; }
        public string? Industry { get; set; } //==BussinusCategoryx
        public string? TaxId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public ICollection<Shipment> Shipments { get; set; }
        public ICollection<ChatMessage> ReceivedMessages { get; set; }
        public ICollection<BankAccount> BankAccount { get; set; } 
        public ICollection<Payment> PaymentsMade { get; set; }

    }
}
