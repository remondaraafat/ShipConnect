namespace ShipConnect.DTOs.OfferDTOs
{
    public class ShipmentWithOffersDTO
    {
        public string ShipmentCode { get; set; }

        public List<ShipmentOffers> Offers { get; set; }
    }
}
