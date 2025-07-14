using static ShipConnect.Enums.Enums;

namespace ShipConnect.DTOs.ShippingCompanies
{
    public class CreateShippingCompanyDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string? Description { get; set; }
        //public string? City { get; set; }
        //public string? Email { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Website { get; set; }
        public string? LicenseNumber { get; set; }
        public string UserId { get; set; } = string.Empty;
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public string? TaxId { get; set; }
    }

}
