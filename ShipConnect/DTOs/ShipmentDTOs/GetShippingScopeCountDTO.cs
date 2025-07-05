namespace ShipConnect.DTOs.ShipmentDTOs
{
    public class GetShippingScopeCountDTO
    {
        public int Domestic {  get; set; }
        public int International { get; set; }

        public int TotalCount { get; set; }
    }
}
