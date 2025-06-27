using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Models
{
    public class Rating:BaseEntity
    {
        public string UserId { get; set; }                  
        public string RatedUserId { get; set; }             
        public int ShipmentId { get; set; }
        [Required]
        [Range(1, 5)]
        public int Score { get; set; }
        [MaxLength(1000)]
        public string? Comment { get; set; }
        
        [ForeignKey("UserId")]
        [InverseProperty(nameof(ApplicationUser.User))]
        public ApplicationUser User { get; set; }  // اللي عمل التقييم

        [ForeignKey("RatedUserId")]
        [InverseProperty(nameof(ApplicationUser.RatedUser))]
        public virtual ApplicationUser RatedUser { get; set; } // اللي اتقيم

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }
    }
}
