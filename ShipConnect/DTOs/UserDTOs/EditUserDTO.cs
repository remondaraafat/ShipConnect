using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.UserDTOs
{
    public class EditUserDTO
    {
        [Required(ErrorMessage = "Startup name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Startup name must be between 2 and 100 characters.")]
        public string StartupName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number can't exceed 20 characters.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
        public IFormFile ProfileImageFile { get; set; }
    }
}
