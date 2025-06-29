using System.ComponentModel.DataAnnotations;

namespace ShipConnect.DTOs.AccountDTOs
{
    public class RegisterAsStartUpDTO:RegisterDTO
    {
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Business category cannot exceed 100 characters.")]
        [Display(Name = "Business Category")]
        public string BusinessCategory { get; set; }
    }
}
