using System.ComponentModel.DataAnnotations;
using MediatR;
using ShipConnect.Helpers;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.CQRS.Register.Commands
{
    public class RegisterAsShippingCompanyCommand:IRequest<GeneralResponse<List<string>>>
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
        public string? TaxId { get; set; }
        public TransportType TransportType { get; set; }
        public ShippingScope ShippingScope { get; set; }
        public string? LicenseNumber { get; set; }
    }

}
