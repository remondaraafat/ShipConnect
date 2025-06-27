using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Hosting;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.Models
{
    public class Shipment:BaseEntity
    {
        public string Code { get; set; }
        public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
        public string? Description { get; set; }
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public double WeightKg { get; set; }  // وزن الشحنة بالكيلو
        public string? Dimensions { get; set; }
        public int Quantity { get; set; }
        public decimal? MinPrice { get; set; }
        public string? SenderAddress { get; set; }//عنوان الارسال
        public DateTime? SentDate { get; set; }//تاريخ الارسال
        public DateTime RequestedPickupDate { get; set; }//تاريخ الاستلام المطلوب
        public DateTime? ActualDelivery { get; set; }//تاريخ التسليم الفعلي
        public string? Packaging { get; set; }
        public string? VehicleType { get; set; }
        public string? Period { get; set; }
        
        public int StartupId { get; set; }
        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public Receiver? Receiver { get; set; }

        

        public ICollection<Tracking> Trackings { get; set; }
        public ICollection<Offer> Offers { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
        
        public ICollection<Payment> Payments { get; set; }
        // reviewed
        [ForeignKey("StartupId")]
        public StartUp? Startup { get; set; }

    }
}
