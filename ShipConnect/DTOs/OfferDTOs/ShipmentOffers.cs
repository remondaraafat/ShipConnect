namespace ShipConnect.DTOs.OfferDTOs
{
    public class ShipmentOffers:OfferDTO
    {
        public int OfferId { get; set; }
        public double CompanyRating { get; set; }
        public string? CompanyName { get; set; }

    }
}
