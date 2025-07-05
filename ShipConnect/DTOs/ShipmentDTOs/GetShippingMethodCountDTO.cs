namespace ShipConnect.DTOs.ShipmentDTOs
{
    public class GetShippingMethodCountDTO
    {
        public int Land {  get; set; }
        public int Sea { get; set; }
        public int Air { get; set; }

        public int TotalCount { get; set; }
    }
}
