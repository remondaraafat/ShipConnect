using System.ComponentModel.DataAnnotations;
using static ShipConnect.Enums.Enums;

namespace ShipConnect.DTOs.AccountDTOs
{
    public class RegisterAsShippingCompanyDTO
    {
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Company name cannot exceed 100 characters.")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "City name cannot exceed 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "*")]
        [MaxLength(300, ErrorMessage = "Address cannot exceed 300 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain upper, lower case letters, and at least one number.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "*")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "*")]
        [EnumDataType(typeof(TransportType), ErrorMessage = "Invalid transport type.")]
        public TransportType TransportType { get; set; }

        [Required(ErrorMessage = "*")]
        [EnumDataType(typeof(ShippingScope), ErrorMessage = "Invalid shipping scope.")]
        public ShippingScope ShippingScope { get; set; }

        [MaxLength(100, ErrorMessage = "License number cannot exceed 100 characters.")]
        public string? LicenseNumber { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [MaxLength(100, ErrorMessage = "Tax ID cannot exceed 100 characters.")]
        public string? TaxId { get; set; }
    }
}
