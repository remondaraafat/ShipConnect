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
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }


        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; set; }
    }
}
