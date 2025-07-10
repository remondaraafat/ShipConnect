using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.ShipmentDTOs
{
    public class GetShipmentByIdDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime RequestDate { get; set; }
        public double WeightKg { get; set; }  // وزن الشحنة بالكيلو
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Dimensions { get; set; }
        public string ShipmentType { get; set; } // نوع الشحنة (مثلاً: أجهزة، أوراق، ...)
        public string Status { get; set; }
        public string? Packaging { get; set; }
        public string PackagingOptions { get; set; }
        public string? Description { get; set; }
        public string DestinationAddress { get; set; }
        public string ShippingScope { get; set; }
        public string TransportType { get; set; }
        public DateTime RequestedPickupDate { get; set; }//تاريخ الاستلام المطلوب
        public int OffersCount { get; set; }

        //sender data
        public string? SenderPhone { get; set; }
        public string? SenderAddress { get; set; }//عنوان الارسال
        public DateTime SentDate { get; set; }//تاريخ الارسال

        //receiver data
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverPhone { get; set; } 
        public string? ReceiverEmail { get; set; }

        public string? CompanyName { get; set; }
        public DateTime? ActualDelivery { get; set; }//تاريخ التسليم الفعلي
        public int offerId { get; set; }

        //rating
        public int RatingScore { get; set; }

    }
}
