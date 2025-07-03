namespace ShipConnect.DTOs.OfferDTOs
{
    public class UpdateOfferDto
    {
        public decimal Price { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public string? Notes { get; set; }
        public bool IsAccepted { get; set; }
    }
}
