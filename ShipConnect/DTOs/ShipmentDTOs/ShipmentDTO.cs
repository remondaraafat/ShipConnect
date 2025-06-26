namespace ShipConnect.DTOs.ShipmentDTOs
{
    public class ShipmentDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public DateTime ShippingDate { get; set; }
        public string Status { get; set; }
        public int StartUpId { get; set; }
        public int ShippingCompanyId { get; set; }
    }
}
