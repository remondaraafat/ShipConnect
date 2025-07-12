namespace ShipConnect.DTOs.OfferDTOs
{
    public class OfferDTO
    {
        public decimal Price { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public string? Notes { get; set; }
    }
}
