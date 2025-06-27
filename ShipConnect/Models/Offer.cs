using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShipConnect.RepositoryContract;

namespace ShipConnect.Models
{
    public class Offer:BaseEntity
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        public int EstimatedDeliveryDays { get; set; }  // مدة التوصيل المتوقعة
        public string? Notes { get; set; }
        public bool IsAccepted { get; set; } = false;
        public int ShipmentId { get; set; }
        public int ShippingCompanyId { get; set; }

        [ForeignKey("ShipmentId")]
        public Shipment? Shipment { get; set; }

        [ForeignKey("ShippingCompanyId")]
        public ShippingCompany? ShippingCompany { get; set; }

    }
}
