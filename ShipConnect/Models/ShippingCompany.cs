using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.Models
{
    public class ShippingCompany:BaseEntity
    {
        [Required]
        public string CompanyName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? City { get; set; }
        public string Address { get; set; } = string.Empty;
        [Phone]
        public string Phone { get; set; } = string.Empty;
        [StringLength(100)]
        public string? Website { get; set; }
        public string? LicenseNumber { get; set; }
        public string UserId { get; set; }
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public string? TaxId { get; set; }

        // edited and reviewd
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        
        //public ICollection<BankAccount> BankAccounts { get; set; }

        public ICollection<Offer> ShippingOffers { get; set; }
        //public ICollection<ChatMessage> SentMessages { get; set; } 
        public ICollection<Tracking> Trackings { get; set; }
        public ICollection<Rating> Ratings { get; set; } 
    }
}
