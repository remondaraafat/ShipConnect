namespace ShipConnect.DTOs.ShipmentDTOs
{
    public class ShipmentDetailsDTO
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
        //public string? Packaging { get; set; }
        public string PackagingOptions { get; set; }
        public string? Description { get; set; }
        public string DestinationAddress { get; set; }
        public string ShippingScope { get; set; }
        public string TransportType { get; set; }
        public DateTime RequestedPickupDate { get; set; }//تاريخ الاستلام المطلوب
        public int? OffersCount { get; set; }

        //sender data
        public string? SenderAddress { get; set; }//عنوان الارسال
        public DateTime SentDate { get; set; }//تاريخ الارسال


        public string? CompanyName { get; set; }
    }
}
