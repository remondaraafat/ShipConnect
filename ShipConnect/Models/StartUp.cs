﻿using System.ComponentModel.DataAnnotations;
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
        //public string? City { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public string? Website { get; set; }
        [StringLength(100)]
        public string BusinessCategory { get; set; }
        public string? TaxId { get; set; }
        public string UserId { get; set; }

        //public ICollection<ChatMessage> ReceivedMessages { get; set; }
        //public ICollection<BankAccount> BankAccount { get; set; } 
        
        // reviewed
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public ICollection<Shipment> Shipments { get; set; }
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    }
}
