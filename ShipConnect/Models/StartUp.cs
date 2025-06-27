using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipConnect.Models
{
    public class StartUp:BaseEntity
    {
        [Required]
        public string CompanyName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? Industry { get; set; } //==BussinusCategoryx
        public string? TaxId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public ICollection<Shipment> Shipments { get; set; }
        public ICollection<ChatMessage> ReceivedMessages { get; set; }
        public ICollection<BankAccount> BankAccount { get; set; } 
        public ICollection<Payment> PaymentsMade { get; set; }

    }
}
