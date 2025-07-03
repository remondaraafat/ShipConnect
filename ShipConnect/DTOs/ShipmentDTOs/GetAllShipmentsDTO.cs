namespace ShipConnect.DTOs.ShipmentDTOs
{
    public class GetAllShipmentsDTO
    {
        public int Id { get; set; } 
        public int Code { get; set; }
        public string Status { get; set; }
        public DateTime RequestedPickupDate { get; set; }
    }
}
