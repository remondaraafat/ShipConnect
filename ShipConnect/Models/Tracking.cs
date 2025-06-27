using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.Models
{
    public class Tracking:BaseEntity
    {
        public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
        [MaxLength(500)]
        public string? Notes { get; set; }
        public string? Location { get; set; }
        public int ShipmentId { get; set; }



        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; set; }
    }
}
