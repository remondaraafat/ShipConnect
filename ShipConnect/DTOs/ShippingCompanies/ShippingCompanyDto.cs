using static ShipConnect.Enums.Enums;

namespace ShipConnect.DTOs.ShippingCompanies
{
    public class ShippingCompanyDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = "/images/default-user.png";//default image
        public string? Email {  get; set; } 
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public string? Website { get; set; }
        public string? LicenseNumber { get; set; }
        public string? TaxId { get; set; }
    }
}
