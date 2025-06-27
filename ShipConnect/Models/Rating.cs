using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Models
{
    public class Rating:BaseEntity
    {
        [Key]
        public int UserId { get; set; }                  
        [Key]
        public int RatedUserId { get; set; }             
        [Key]
        public int ShipmentId { get; set; }
        [Required]
        [Range(1, 5)]
        public int Score { get; set; }
        [MaxLength(1000)]
        public string? Comment { get; set; }
        
        [ForeignKey("UserId")]
        [InverseProperty("User")]
        public ApplicationUser User { get; set; }  // اللي عمل التقييم

        [ForeignKey("RatedUserId")]
        [InverseProperty("RatedUser")]
        public virtual ApplicationUser RatedUser { get; set; } // اللي اتقيم

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }
    }
}
