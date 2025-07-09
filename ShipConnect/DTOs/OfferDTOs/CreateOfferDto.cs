namespace ShipConnect.DTOs.OfferDTOs
{
    public class CreateOfferDto
    {
        public decimal Price { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public string? Notes { get; set; }
        public int ShipmentId { get; set; }
    }
}
