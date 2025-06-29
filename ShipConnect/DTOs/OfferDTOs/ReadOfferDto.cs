namespace ShipConnect.DTOs.OfferDTOs
{
    public class ReadOfferDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public string? Notes { get; set; }
        public bool IsAccepted { get; set; }
        public int ShipmentId { get; set; }
        public int ShippingCompanyId { get; set; }
    }

}
