using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.StartUpDTOs
{
    public class EditStartupDTO
    {

        [Required(ErrorMessage = "Startup name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Startup name must be between 2 and 100 characters.")]
        public string StartupName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters.")]
        public string? Description { get; set; }

        [StringLength(100, ErrorMessage = "Business category can't exceed 100 characters.")]
        public string? BusinessCategory { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address can't exceed 200 characters.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number can't exceed 20 characters.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
    }


}

