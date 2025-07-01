using System.ComponentModel.DataAnnotations;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.DTOs.AccountDTOs
{
    public class RegisterAsShippingCompanyDTO:RegisterDTO
    {
        [Required(ErrorMessage = "*")]
        [EnumDataType(typeof(TransportType), ErrorMessage = "Invalid transport type.")]
        public TransportType TransportType { get; set; }

        [Required(ErrorMessage = "*")]
        [EnumDataType(typeof(ShippingScope), ErrorMessage = "Invalid shipping scope.")]
        public ShippingScope ShippingScope { get; set; }

        [MaxLength(100, ErrorMessage = "License number cannot exceed 100 characters.")]
        public string? LicenseNumber { get; set; }
    }
}
