using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Models
{
    public class Rating:BaseEntity
    {
        public int StartUpId { get; set; }                  
        public int CompanyId { get; set; }             
        public int OfferId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Score { get; set; }
        [MaxLength(1000)]
        public string? Comment { get; set; }
        
        [ForeignKey("StartUpId")]
       // [InverseProperty(nameof(StartUp))]
        public StartUp StartUp { get; set; }  // اللي عمل التقييم

        [ForeignKey("CompanyId")]
        //[InverseProperty(nameof(ShippingCompany))]
        public ShippingCompany ShippingCompany { get; set; } // اللي اتقيم

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; }
    }
}
