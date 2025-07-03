using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Hosting;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.Models
{
    public class Shipment:BaseEntity
    {
        [Required] 
        public string Code { get; set; }

        //[MaxLength(100)]
        //public string Title { get; set; }
        public double WeightKg { get; set; }  // وزن الشحنة بالكيلو
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        
        [MaxLength(100)]
        public string? Dimensions { get; set; }

        [MaxLength(300)]
        public string DestinationAddress { get; set; }

        //[MaxLength(100)]
        //public string DestinationCity { get; set; }
        public string ShipmentType { get; set; } // نوع الشحنة (مثلاً: أجهزة، أوراق، ...)
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public DateTime RequestedPickupDate { get; set; }//تاريخ الاستلام المطلوب
        public string? Packaging { get; set; }
        public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;
        public PackagingOptions PackagingOptions { get; set; }
        public string? Description { get; set; }
        //sender data
        public string? SenderPhone { get; set; }
        //public string? SenderCity { get; set; }
        public string? SenderAddress { get; set; }//عنوان الارسال
        public DateTime SentDate { get; set; }//تاريخ الارسال

        //public string? ReceiverNotes { get; set; }
        public DateTime? ActualDelivery { get; set; }//تاريخ التسليم الفعلي
        public int StartupId { get; set; }
        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public Receiver? Receiver { get; set; }

        // reviewed
        [ForeignKey("StartupId")]
        public StartUp? Startup { get; set; }

        public ICollection<Tracking> Trackings { get; set; }
        public ICollection<Offer> Offers { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
